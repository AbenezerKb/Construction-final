using System.Globalization;
using Microsoft.EntityFrameworkCore;
using ERP.Exceptions;
using ERP.Context;
using ERP.Models;
using ERP.DTOs.WeeklyPlan;

namespace ERP.Services.WeeklyPlanService
{

    public class WeeklyPlanService : IWeeklyPlanService
    {
        private readonly DataContext dbContext;

        public WeeklyPlanService(DataContext context)
        {
            dbContext = context;
        }

        private Task<List<int>> GetDifference(List<int> sample, List<int> all)
        {
            return Task.FromResult(sample.Where(s => !all.Contains(s)).ToList());
        }

        private async Task<bool> PlanExists(WeeklyPlanDto weeklyPlanDto)
        {
            // var weekEndDate = weeklyPlanDto.WeekStartDate.AddDays(7);
            var plans = await dbContext.WeeklyPlans
                      .AsNoTracking().Where(p => p.ProjectId == weeklyPlanDto.ProjectId)
                      .ToListAsync();
            // return plans.Any(p => p.WeekStartDate >= weeklyPlanDto.WeekStartDate
            //            && p.WeekStartDate.AddDays(7) <= weekEndDate);
            return plans.Any(p => ISOWeek.GetWeekOfYear(p.WeekStartDate) == ISOWeek.GetWeekOfYear(weeklyPlanDto.WeekStartDate));
        }
        public async Task<WeeklyPlan> Add(WeeklyPlanDto weeklyPlanDto)
        {

            var project = await dbContext.Projects.FindAsync(weeklyPlanDto.ProjectId);
            if (project == null) throw new ItemNotFoundException($"Project not found with PrjectId={weeklyPlanDto.ProjectId}");
            if (await PlanExists(weeklyPlanDto)) throw new ItemAlreadyExistException($"Weekly Plan already exist with WeekStartDate=${weeklyPlanDto.WeekStartDate} and ProjectId={weeklyPlanDto.ProjectId}");


            if (!await dbContext.Projects.AnyAsync(p => p.Id == weeklyPlanDto.ProjectId)) throw new ItemNotFoundException($"Project not found with projectId={weeklyPlanDto.ProjectId}");

            var unknownIds = await GetDifference(
                  weeklyPlanDto.PlanValues.Select(pv => pv.SubTaskId).ToList(),
                          await dbContext.SubTasks.Select(s => s.Id).ToListAsync()
                    );

            if (unknownIds.Count != 0) throw new ItemNotFoundException($"Task(s) not found with id=[{string.Join(',', unknownIds)}]");
            //TODO: Check for invalid employee Id as well

            List<WeeklyPlanValue> planValues = new();

            weeklyPlanDto.PlanValues.ForEach(wp =>
            {
                WeeklyPlanValue value = new WeeklyPlanValue
                {
                    SubTaskId = wp.SubTaskId,
                };
                if (wp.PerformedBy != null)
                {
                    value.PerformedBy = wp.PerformedBy;
                }
                else
                {
                    value.SubContractorId = wp.SubContractorId;
                }

                planValues.Add(value);
            });

            var newPlan = new WeeklyPlan
            {
                WeekNo = ISOWeek.GetWeekOfYear(weeklyPlanDto.WeekStartDate),
                WeekStartDate = weeklyPlanDto.WeekStartDate,
                Remark = weeklyPlanDto.Remark,
                ProjectId = weeklyPlanDto.ProjectId,
                PlanValues = planValues,

            };

            await dbContext.WeeklyPlans.AddAsync(newPlan);

            // await dbContext.Notifications.AddAsync(new Notification
            // {
            //     Title = "Plan added",
            //     Type = "Weekly Plan",
            // });
            //TODO: Add 'Weekly Plan Sent' Notification Here
            await dbContext.SaveChangesAsync();

            return newPlan;

        }

        public async Task<WeeklyPlanValue> AddTask(int weeklyPlanId, WeeklyPlanValueDto weeklyPlanValueDto)
        {
            var weeklyPlan = await GetById(weeklyPlanId);
            var subTask = await dbContext.SubTasks.FindAsync(weeklyPlanValueDto.SubTaskId);

            if (subTask == null) throw new ItemNotFoundException($"SubTask not found with subTaskId={weeklyPlanValueDto.SubTaskId}");
            if (weeklyPlan.PlanValues.Any(t => t.SubTaskId == weeklyPlanValueDto.SubTaskId))
                throw new ItemAlreadyExistException($"SubTask with id={weeklyPlanValueDto.SubTaskId} has already been added to weekly plan");
            //TODO: check if employee exists with the given id
            var planValue = new WeeklyPlanValue
            {
                SubTaskId = weeklyPlanValueDto.SubTaskId,
                WeeklyPlanId = weeklyPlanId
            };
            if (weeklyPlanValueDto.PerformedBy != null)
            {
                planValue.PerformedBy = weeklyPlanValueDto.PerformedBy;
            }
            else
            {
                planValue.SubContractorId = weeklyPlanValueDto.SubContractorId;
            }

            weeklyPlan.PlanValues.Add(planValue);

            await dbContext.SaveChangesAsync();
            return planValue;
        }


        public async Task<WeeklyPlan> GetById(int weeklyPlanId)
        {
            var weeklyPlan = await dbContext.WeeklyPlans.Where(wp => wp.Id == weeklyPlanId)
                                                        .Include(wp => wp.PlanValues)
                                                        .ThenInclude(wpv => wpv.SubTask)
                                                        .FirstOrDefaultAsync();
            if (weeklyPlan == null) throw new ItemNotFoundException($"Weekly plan not found with WeeklyPlanId={weeklyPlanId}");
            return weeklyPlan;
        }

        public async Task<List<WeeklyPlan>> GetByMonthYear(int month, int year, int projectId)
        {

            var plans = await dbContext.WeeklyPlans.Where(wp => wp.ProjectId == projectId && wp.WeekStartDate.Month == month && wp.WeekStartDate.Year == year)
                                                         .Include(p => p.PlanValues)
                                                         .ThenInclude(wpv => wpv.SubTask)
                                                         .OrderBy(wp => wp.WeekStartDate)
                                                         .ToListAsync();

            if (plans == null || plans.Count == 0) throw new ItemNotFoundException($"Monthly plan not found with week={month} and year={year}");
            return plans;
        }

        public async Task<List<WeeklyPlan>> GetByProjectId(int projectId)
        {
            List<WeeklyPlan> weeklyPlans = await dbContext.WeeklyPlans.Where(wp => wp.ProjectId == projectId)
                                                                      .Include(wp => wp.PlanValues)
                                                                      .ThenInclude(wpv => wpv.SubTask)
                                                                      .ToListAsync();
            if (weeklyPlans == null || !weeklyPlans.Any()) throw new ItemNotFoundException($"Weekly Plans not found with ProjectId={projectId}");
            return weeklyPlans;
        }

        public async Task<WeeklyPlan> GetByWeekAndYear(int week, int year, int projectId)
        {
            var weeklyPlan = (await dbContext.WeeklyPlans.Where(p => p.ProjectId == projectId && p.WeekNo == week && p.WeekStartDate.Year == year)
                                                        .Include(p => p.PlanValues)
                                                        .ThenInclude(wpv => wpv.SubTask)
                                                        .ToListAsync())
                                                        .FirstOrDefault();

            if (weeklyPlan == null) throw new ItemNotFoundException($"Weekly plan not found with week={week} and year={year}");
            return weeklyPlan;
        }

        public async Task<WeeklyPlan> Remove(int weeklyPlanId)
        {
            var weeklyPlan = await GetById(weeklyPlanId);
            dbContext.WeeklyPlans.Remove(weeklyPlan);
            await dbContext.SaveChangesAsync();
            return weeklyPlan;
        }

        public async Task<WeeklyPlanValue> RemovePlannedTask(int subTaskId, int weeklyPlanId)
        {
            var weeklyPlan = await GetById(weeklyPlanId);
            var planValue = weeklyPlan.PlanValues.ToList().Find(wp => wp.SubTaskId == subTaskId);


            if (planValue == null) throw new ItemNotFoundException($"WeeklyPlanValue not found by subTaskId={subTaskId}");

            weeklyPlan.PlanValues.Remove(planValue);
            await dbContext.SaveChangesAsync();
            return planValue;
        }

        public async Task<WeeklyPlan> Update(int planId, WeeklyPlanDto weeklyPlanDto)
        {
            var project = await dbContext.Projects.FindAsync(weeklyPlanDto.ProjectId);
            if (project == null) throw new ItemNotFoundException($"Project not found with PrjectId={weeklyPlanDto.ProjectId}");
            var weeklyPlan = await GetById(planId);

            if (weeklyPlan.WeekStartDate != weeklyPlanDto.WeekStartDate)
            {
                weeklyPlan.WeekStartDate = weeklyPlanDto.WeekStartDate;
                weeklyPlan.WeekNo = ISOWeek.GetWeekOfYear(weeklyPlanDto.WeekStartDate);

            }
            weeklyPlan.Remark = weeklyPlanDto.Remark;
            await dbContext.SaveChangesAsync();
            return weeklyPlan;
        }
    }
}