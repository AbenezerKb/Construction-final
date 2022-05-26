import { useState, useMemo, useEffect, useCallback } from "react";
import { Form, Button, Container, Table as BootstrapTable } from "react-bootstrap";
import { Link } from "react-router-dom";
import { FaFolderOpen } from "react-icons/fa";
import Table from "../../fragments/Table";
import LoadingSpinner from "../../fragments/LoadingSpinner";
import ConnectionError from "../../fragments/ConnectionError";
import FormRow from "../../fragments/FormRow";
import { useQuery, useQueryClient } from "react-query";
import { fetchSites } from "../../../api/site";
import { ITEMTYPE, ISSUEREPORTGROUPBY, ISSUESTATUS } from "../../../Constants";
import { fetchIssueReport } from "../../../api/report";
import { fetchEmployees } from "../../../api/employee";
import IssueStatusBadge from "../issue/fragments/IssueStatusBadge";
import { fetchMaterials } from "../../../api/item";
import Chart from "../../fragments/Chart";

function initFormValues() {
    return {
        dateOf: "1",
        fromDate: "",
        toDate: "",
        siteId: "-1",
        status: "-1",
        itemId: "-1",
        employeeRole: "-1",
        employeeId: "-1",
        groupBy: ISSUEREPORTGROUPBY.STATUS.key,
    };
}

function IssueReport() {
    const [formValues, setFormValues] = useState(initFormValues);
    const [report, setReport] = useState();
    const [chartData, setChartData] = useState();
    const [tableData, setTableData] = useState();
    const [chartType, setChartType] = useState("bar");
    const queryClient = useQueryClient();

    const sitesQuery = useQuery(["sites"], fetchSites);
    var materialsQuery = useQuery(["materials"], fetchMaterials);
    const employeesQuery = useQuery(["employees"], fetchEmployees);

    const {
        data: queryResult,
        isLoading,
        isFetching,
        isError,
        error,
        refetch,
    } = useQuery(["issue", formValues], () => fetchIssueReport(formValues), {
        enabled: false,
    });

    function valueChanged(e) {
        queryClient.removeQueries(["issue", formValues]);
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

                if (groupBy === ISSUEREPORTGROUPBY.STATUS.key) label = ISSUESTATUS[current.key];
                else if (
                    groupBy === ISSUEREPORTGROUPBY.REQUESTEDBY.key ||
                    groupBy === ISSUEREPORTGROUPBY.APPROVEDBY.key ||
                    groupBy === ISSUEREPORTGROUPBY.HANDEDBY.key
                )
                    label = current.value ? `${current.value.fName} ${current.value.mName}` : "N/A";
                else if (groupBy === ISSUEREPORTGROUPBY.SITE.key) label = current.value.name;

                return { ...current, label };
            });

            const data = {
                labels: summary.map((x) => x.label),
                datasets: [
                    {
                        label: "Issues",
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
            data[key].statusBadge = IssueStatusBadge(data[key].status);

            //add links to each row
            data[key].open = (
                <>
                    <Link className="btn btn-teal me-2" to={`/issue/${data[key].issueId}`}>
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
                accessor: "issueId", // accessor is the "key" in the data
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
                Header: "Haded Over On",
                accessor: "handDate",
            },
            {
                Header: "Handed Over By",
                accessor: "handByName",
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
                                {Object.keys(ISSUESTATUS).map((key) =>
                                    !isNaN(key) ? (
                                        <option key={key} value={key}>
                                            {ISSUESTATUS[key]}
                                        </option>
                                    ) : (
                                        ""
                                    )
                                )}
                            </Form.Select>
                        }
                        labelR="Issue Status"
                        valueR={
                            <Form.Select
                                name="status"
                                value={formValues["status"]}
                                onChange={valueChanged}
                            >
                                <option value="-1">All</option>
                                {Object.keys(ISSUESTATUS).map((key) =>
                                    !isNaN(key) ? (
                                        <option key={key} value={key}>
                                            {ISSUESTATUS[key]}
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
                                {Object.keys(ISSUESTATUS).map((key) =>
                                    !isNaN(key) ? (
                                        <option key={key} value={key}>
                                            {ISSUESTATUS[key]} By
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
                        labelL="Item"
                        valueL={
                            <Form.Select
                                name="itemId"
                                value={formValues["itemId"]}
                                onChange={valueChanged}
                                required
                            >
                                <option value="-1">All Materials</option>
                                {materialsQuery.data &&
                                    materialsQuery.data?.map((material) => (
                                        <option key={material.itemId} value={material.itemId}>
                                            {material.name}
                                        </option>
                                    ))}
                            </Form.Select>
                        }
                        valueR={null}
                    />

                    <FormRow
                        labelL="Group By"
                        valueL={
                            <Form.Select
                                name="groupBy"
                                value={formValues["groupBy"]}
                                onChange={valueChanged}
                            >
                                {Object.keys(ISSUEREPORTGROUPBY).map((i) => (
                                    <option key={i} value={ISSUEREPORTGROUPBY[i].key}>
                                        {ISSUEREPORTGROUPBY[i].label}
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
                            <h1 className="text-center mb-2 fs-4">Issue Report Summary</h1>
                            <BootstrapTable responsive striped bordered hover>
                                <thead>
                                    <tr>
                                        <th>#</th>
                                        <th>
                                            {
                                                ISSUEREPORTGROUPBY[
                                                    Object.keys(ISSUEREPORTGROUPBY).find(
                                                        (i) =>
                                                            ISSUEREPORTGROUPBY[i].key ===
                                                            formValues.groupBy
                                                    )
                                                ]?.label
                                            }
                                        </th>
                                        <th>Number of Issues</th>
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

export default IssueReport;
