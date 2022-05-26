import { useState, useMemo, useEffect, useCallback } from "react";
import { Form, Button, Container, Table as BootstrapTable } from "react-bootstrap";
import { Link } from "react-router-dom";
import { FaFolderOpen } from "react-icons/fa";
import Table from "../../fragments/Table";
import Header from "../../layouts/Header";

import LoadingSpinner from "../../fragments/LoadingSpinner";
import ConnectionError from "../../fragments/ConnectionError";
import FormRow from "../../fragments/FormRow";
import { useQuery, useQueryClient } from "react-query";
import { fetchSites } from "../../../api/site";
import { BORROWREPORTGROUPBY, BORROWSTATUS } from "../../../Constants";
import { fetchEquipmentCategories, fetchEquipmentCategory } from "../../../api/category";
import { fetchBorrowReport } from "../../../api/report";
import { fetchEmployees } from "../../../api/employee";
import BorrowStatusBadge from "../borrow/fragments/BorrowStatusBadge";
import Chart from "../../fragments/Chart";

function initFormValues() {
    return {
        dateOf: "1",
        fromDate: "",
        toDate: "",
        siteId: "-1",
        status: "-1",
        equipmentCategoryId: "-1",
        itemId: "-1",
        employeeRole: "-1",
        employeeId: "-1",
        groupBy: BORROWREPORTGROUPBY.STATUS.key,
    };
}

function BorrowReport() {
    const [formValues, setFormValues] = useState(initFormValues);
    const [report, setReport] = useState();
    const [chartData, setChartData] = useState();
    const [tableData, setTableData] = useState();
    const [chartType, setChartType] = useState("bar");
    const queryClient = useQueryClient();

    const sitesQuery = useQuery(["sites"], fetchSites);
    const categoriesQuery = useQuery("equipmentcategories", fetchEquipmentCategories);
    var equipmentsQuery = useQuery(
        ["equipmentcategory", formValues.equipmentCategoryId],
        () =>
            formValues.equipmentCategoryId !== "-1" &&
            formValues.equipmentCategoryId !== undefined &&
            fetchEquipmentCategory(formValues.equipmentCategoryId)
    );
    const employeesQuery = useQuery(["employees"], fetchEmployees);

    const {
        data: queryResult,
        isLoading,
        isFetching,
        isError,
        error,
        refetch,
    } = useQuery(["borrow", formValues], () => fetchBorrowReport(formValues), {
        enabled: false,
    });

    function valueChanged(e) {
        queryClient.removeQueries(["borrow", formValues]);
        setFormValues({ ...formValues, [e.target.name]: e.target.value });
        setChartData(null);
        setTableData(null);
        setReport(null);
    }

    const setChartDataCallback = useCallback(
        (summary) => {
            const { groupBy } = formValues;
            summary = summary.map((current) => {
                var label = current.value;

                if (groupBy === BORROWREPORTGROUPBY.STATUS.key) label = BORROWSTATUS[current.key];
                else if (
                    groupBy === BORROWREPORTGROUPBY.REQUESTEDBY.key ||
                    groupBy === BORROWREPORTGROUPBY.APPROVEDBY.key ||
                    groupBy === BORROWREPORTGROUPBY.HANDEDBY.key
                )
                    label = current.value ? `${current.value.fName} ${current.value.mName}` : "N/A";
                else if (groupBy === BORROWREPORTGROUPBY.SITE.key) label = current.value.name;

                return { ...current, label };
            });

            const data = {
                labels: summary.map((x) => x.label),
                datasets: [
                    {
                        label: "Borrows",
                        data: summary.map((x) => x.count),
                    },
                ],
            };

            setTableData(summary);
            setChartData(data);
        },
        [formValues.groupBy]
    );

    useEffect(() => {
        if (queryResult === undefined) return;

        const { data } = queryResult;

        setChartDataCallback(queryResult.summary);

        Object.keys(data).map((key, index) => {
            //format date
            data[key].requestDate = new Date(data[key].requestDate).toLocaleDateString();
            data[key].approveDate =
                data[key].approveDate && new Date(data[key].approveDate).toLocaleDateString();
            data[key].handDate =
                data[key].handDate && new Date(data[key].handDate).toLocaleDateString();

            //format names
            data[
                key
            ].requestedByName = `${data[key].requestedBy.fName} ${data[key].requestedBy.mName}`;

            data[key].approvedByName =
                data[key].approvedBy &&
                `${data[key].approvedBy.fName} ${data[key].approvedBy.mName}`;

            data[key].handedByName =
                data[key].handedBy && `${data[key].handedBy.fName} ${data[key].handedBy.mName}`;

            //add status badge
            data[key].statusBadge = BorrowStatusBadge(data[key].status);

            //add links to each row
            data[key].open = (
                <>
                    <Link className="btn btn-teal me-2" to={`/borrow/${data[key].borrowId}`}>
                        <FaFolderOpen className="me-1 mb-1" />
                    </Link>
                </>
            );

            return null;
        });

        setReport(data);
    }, [queryResult]);

    const columns = useMemo(
        () => [
            {
                Header: "No.",
                accessor: "borrowId", // accessor is the "key" in the data
            },

            {
                Header: "Status",
                accessor: "statusBadge",
                sortType: (
                    rowA,
                    rowB //get original data of row and compare status
                ) => (rowA.original.status > rowB.original.status ? 1 : -1),
            },
            {
                Header: "Requested On",
                accessor: "requestDate",
            },
            {
                Header: "Requested By",
                accessor: "requestedByName",
            },
            {
                Header: "Approved On",
                accessor: "approveDate",
            },
            {
                Header: "Approved By",
                accessor: "approvedByName",
            },
            {
                Header: "Handed Over On",
                accessor: "handDate",
            },
            {
                Header: "Handed Over By",
                accessor: "handedByName",
            },
            {
                Header: "Site",
                accessor: "site.name",
            },
            {
                Header: "",
                accessor: "open", // accessor is the "key" in the data
                disableSortBy: true,
            },
        ],
        []
    );

    return (
        <>
            <Container className="my-3">
                <Form className="mb-3">
                    <FormRow
                        labelL="Date"
                        valueL={
                            <Form.Select
                                name="dateOf"
                                value={formValues["dateOf"]}
                                onChange={valueChanged}
                            >
                                {Object.keys(BORROWSTATUS).map((key) =>
                                    !isNaN(key) ? (
                                        <option key={key} value={key}>
                                            {BORROWSTATUS[key]}
                                        </option>
                                    ) : (
                                        ""
                                    )
                                )}
                            </Form.Select>
                        }
                        labelR="Borrow Status"
                        valueR={
                            <Form.Select
                                name="status"
                                value={formValues["status"]}
                                onChange={valueChanged}
                            >
                                <option value="-1">All</option>
                                {Object.keys(BORROWSTATUS).map((key) =>
                                    !isNaN(key) ? (
                                        <option key={key} value={key}>
                                            {BORROWSTATUS[key]}
                                        </option>
                                    ) : (
                                        ""
                                    )
                                )}
                            </Form.Select>
                        }
                    />
                    <FormRow
                        labelL="From Date"
                        valueL={
                            <Form.Control
                                type="date"
                                name="fromDate"
                                max={formValues["toDate"]}
                                value={formValues["fromDate"]}
                                onChange={valueChanged}
                            />
                        }
                        labelR="To Date"
                        valueR={
                            <Form.Control
                                type="date"
                                name="toDate"
                                min={formValues["fromDate"]}
                                value={formValues["toDate"]}
                                onChange={valueChanged}
                            />
                        }
                    />

                    <FormRow
                        labelL="Site"
                        valueL={
                            <Form.Select
                                name="siteId"
                                value={formValues["siteId"]}
                                onChange={valueChanged}
                            >
                                <option value="-1">All Sites</option>
                                {sitesQuery.data &&
                                    sitesQuery.data?.map((site) => (
                                        <option key={site.siteId} value={site.siteId}>
                                            {site.name}
                                        </option>
                                    ))}
                            </Form.Select>
                        }
                        valueR={null}
                    />

                    <FormRow
                        labelL="Employee Role"
                        valueL={
                            <Form.Select
                                name="employeeRole"
                                value={formValues["employeeRole"]}
                                onChange={valueChanged}
                            >
                                <option value="-1">Any</option>
                                {Object.keys(BORROWSTATUS).map((key) =>
                                    !isNaN(key) ? (
                                        <option key={key} value={key}>
                                            {BORROWSTATUS[key]} By
                                        </option>
                                    ) : (
                                        ""
                                    )
                                )}
                            </Form.Select>
                        }
                        labelR="Employee"
                        valueR={
                            <Form.Select
                                name="employeeId"
                                value={formValues["employeeId"]}
                                onChange={valueChanged}
                            >
                                <option value="-1">All</option>
                                {employeesQuery.data &&
                                    employeesQuery.data?.map((employee) => (
                                        <option
                                            key={employee.employeeId}
                                            value={employee.employeeId}
                                        >
                                            {`${employee.fName} ${employee.mName} ${employee.lName}`}
                                        </option>
                                    ))}
                            </Form.Select>
                        }
                    />

                    <FormRow
                        labelL="Category"
                        valueL={
                            <Form.Select
                                name="equipmentCategoryId"
                                value={formValues["equipmentCategoryId"]}
                                onChange={valueChanged}
                            >
                                <option value="-1">All Categories</option>
                                {categoriesQuery.data?.map((category) => (
                                    <option
                                        key={category.equipmentCategoryId}
                                        value={category.equipmentCategoryId}
                                    >
                                        {category.name}
                                    </option>
                                ))}
                            </Form.Select>
                        }
                        labelR="Item"
                        valueR={
                            <Form.Select
                                name="itemId"
                                value={formValues["itemId"]}
                                onChange={valueChanged}
                                required
                            >
                                <option value="-1">All Equipments</option>
                                {equipmentsQuery.data?.equipments?.map((equipment) => (
                                    <option key={equipment.itemId} value={equipment.itemId}>
                                        {equipment.item.name}
                                    </option>
                                ))}
                            </Form.Select>
                        }
                    />

                    <FormRow
                        labelL="Group By"
                        valueL={
                            <Form.Select
                                name="groupBy"
                                value={formValues["groupBy"]}
                                onChange={valueChanged}
                            >
                                {Object.keys(BORROWREPORTGROUPBY).map((i) => (
                                    <option key={i} value={BORROWREPORTGROUPBY[i].key}>
                                        {BORROWREPORTGROUPBY[i].label}
                                    </option>
                                ))}
                            </Form.Select>
                        }
                        labelR="Chart Type"
                        valueR={
                            <Form.Select
                                value={chartType}
                                onChange={(e) => setChartType(e.target.value)}
                            >
                                <option value="bar">Bar Chart</option>
                                <option value="line">Line Chart</option>
                            </Form.Select>
                        }
                    />

                    <div className="row">
                        <div className="col-10 d-grid">
                            <Button className="btn-teal-dark" onClick={refetch}>
                                Generate Report
                            </Button>
                        </div>
                        <div className="col-2 d-grid">
                            <Button
                                variant="outline-danger"
                                onClick={() => {
                                    setFormValues(initFormValues());
                                    setChartData(null);
                                    setTableData(null);
                                    setReport(null);
                                }}
                            >
                                Reset
                            </Button>
                        </div>
                    </div>
                </Form>

                {isError && <ConnectionError status={error?.response?.status} />}

                {(isLoading || isFetching) && <LoadingSpinner />}

                {chartData && (
                    <>
                        <h1 className="display-6 text-center mb-2">Report Result</h1>

                        <Chart data={chartData} type={chartType} />

                        <Container fluid className="px-4">
                            <h1 className="text-center mb-2 fs-4">Borrow Report Summary</h1>
                            <BootstrapTable responsive striped bordered hover>
                                <thead>
                                    <tr>
                                        <th>#</th>
                                        <th>
                                            {
                                                BORROWREPORTGROUPBY[
                                                    Object.keys(BORROWREPORTGROUPBY).find(
                                                        (i) =>
                                                            BORROWREPORTGROUPBY[i].key ===
                                                            formValues.groupBy
                                                    )
                                                ]?.label
                                            }
                                        </th>
                                        <th>Number of Borrows</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {tableData.map((current, index) => (
                                        <tr key={index}>
                                            <td>{index + 1}</td>
                                            <td>{current.label}</td>
                                            <td>{current.count}</td>
                                        </tr>
                                    ))}
                                </tbody>
                            </BootstrapTable>
                        </Container>
                    </>
                )}
                {report && (
                    <div className="d-print-none">
                        <h1 className="display-6 text-center my-4">Detailed List</h1>
                        <Table columns={columns} data={report} />
                    </div>
                )}
            </Container>
        </>
    );
}

export default BorrowReport;
