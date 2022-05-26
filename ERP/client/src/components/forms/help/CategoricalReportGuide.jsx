import { url as host } from "../../../api/api";

function CategoricalReportGuide() {
    return (
        <div>
            <hr />

            <div>
                <h3>1. Click on Reports</h3>
                <img
                    src={`${host}file/screenshot_2f49945e-d80a-430e-af11-bdc9354112bf.jpg`}
                    width="600"
                    alt="Click on Reports"
                />
            </div>

            <div>
                <h3>2. Click on the type of transaction you want a report to be generated</h3>
                <p>The default is the general report but In this case Issue is selected.</p>
                <img
                    src={`${host}file/screenshot_9b732683-a333-4b75-8645-e97b14cf6cc0.jpg`}
                    width="600"
                    alt="Click on the type of transaction you want a report to be generated"
                />
            </div>

            <div>
                <h3>3. Selected the Date you want to limit</h3>
                <p>
                    For example, if Requested is selected the From Date and To Date will limit the
                    Requested Date of the Issue.
                </p>
                <img
                    src={`${host}file/screenshot_efe4c07d-8789-4934-bbca-bef92f3892aa.jpg`}
                    width="600"
                    alt="Selected the Date you want to limit"
                />
            </div>

            <div>
                <h3>4. Selecting Group By</h3>
                <p>
                    Group By defines how the Issues will be grouped, the options include Status,
                    Dates, Weeks, Months, Year and Employees.
                </p>
                <img
                    src={`${host}file/screenshot_de2999d4-ed03-4072-9b50-5d2d7597e490.jpg`}
                    width="600"
                    alt="Selecting Group By"
                />
            </div>

            <div>
                <h3>5. Click on Generate Report</h3>
                <p>
                    All other settings are similar to general reports and Reset can be used to Once
                    the setting are what you want then you can generate the report.
                </p>
                <img
                    src={`${host}file/screenshot_8938e9f4-e95b-4f03-90bb-d1db80f38b99.jpg`}
                    width="600"
                    alt="Click on Generate Report"
                />
            </div>
        </div>
    );
}

export default CategoricalReportGuide;
