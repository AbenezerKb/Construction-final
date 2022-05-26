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
import { ITEMTYPE, RECEIVEREPORTGROUPBY, RECEIVESTATUS } from "../../../Constants";
import { fetchEquipmentCategories, fetchEquipmentCategory } from "../../../api/category";
import { fetchReceiveReport } from "../../../api/report";
import { fetchEmployees } from "../../../api/employee";
import ReceiveStatusBadge from "../receive/fragments/ReceiveStatusBadge";
import { fetchMaterials } from "../../../api/item";
import Chart from "../../fragments/Chart";

function initFormValues() {
    return {
        dateOf: "1",
        fromDate: "",
        toDate: "",
        siteId: "-1",
        itemType: "-1",
        status: "-1",
        equipmentCategoryId: "-1",
        itemId: "-1",
        employeeRole: "-1",
        employeeId: "-1",
        groupBy: RECEIVEREPORTGROUPBY.STATUS.key,
    };
}

function ReceiveReport() {
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
    var materialsQuery = useQuery(["materials"], fetchMaterials);
    const employeesQuery = useQuery(["employees"], fetchEmployees);

    const {
        data: queryResult,
        isLoading,
        isFetching,
        isError,
        error,
        refetch,
    } = useQuery(["receive", formValues], () => fetchReceiveReport(formValues), {
        enabled: false,
    });

    function valueChanged(e) {
        queryClient.removeQueries(["receive", formValues]);
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

                if (groupBy === RECEIVEREPORTGROUPBY.STATUS.key) label = RECEIVESTATUS[current.key];
                else if (
                    groupBy === RECEIVEREPORTGROUPBY.RECEIVEDBY.key ||
                    groupBy === RECEIVEREPORTGROUPBY.APPROVEDBY.key ||
                    groupBy === RECEIVEREPORTGROUPBY.PURCHASEDBY.key
                )
                    label = current.value ? `${current.value.fName} ${current.value.mName}` : "N/A";
                else if (groupBy === RECEIVEREPORTGROUPBY.SITE.key) label = current.value.name;

                return { ...current, label };
            });

            const data = {
                labels: summary.map((x) => x.label),
                datasets: [
                    {
                        label: "Receives",
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
            data[key].receiveDate =
                data[key].receiveDate && new Date(data[key].receiveDate).toLocaleDateString();
            data[key].approveDate =
                data[key].approveDate && new Date(data[key].approveDate).toLocaleDateString();
            data[key].purchaseDate =
                data[key].purchaseDate && new Date(data[key].purchaseDate).toLocaleDateString();

            //format names
            data[key].receivedByName =
                data[key].receivedBy &&
                `${data[key].receivedBy.fName} ${data[key].receivedBy.mName}`;

            data[key].approvedByName =
                data[key].approvedBy &&
                `${data[key].approvedBy.fName} ${data[key].approvedBy.mName}`;

            data[key].purchasedByName =
                data[key].purchasedBy &&
                `${data[key].purchasedBy.fName} ${data[key].purchasedBy.mName}`;

            //add status badge
            data[key].statusBadge = ReceiveStatusBadge(data[key].status);

            //add links to each row
            data[key].open = (
                <>
                    <Link className="btn btn-teal me-2" to={`/receive/${data[key].receiveId}`}>
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
                accessor: "receiveId", // accessor is the "key" in the data
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
                Header: "Received On",
                accessor: "receiveDate",
            },
            {
                Header: "Received By",
                accessor: "receivedByName",
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
                Header: "Purchased On",
                accessor: "purchaseDate",
            },
            {
                Header: "Purchased By",
                accessor: "purchasedByName",
            },
            {
                Header: "Receiving Site",
                accessor: "receivingSite.name",
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
                                {Object.keys(RECEIVESTATUS).map((key) =>
                                    !isNaN(key) ? (
                                        <option key={key} value={key}>
                                            {RECEIVESTATUS[key]}
                                        </option>
                                    ) : (
                                        ""
                                    )
                                )}
                            </Form.Select>
                        }
                        labelR="Receive Status"
                        valueR={
                            <Form.Select
                                name="status"
                                value={formValues["status"]}
                                onChange={valueChanged}
                            >
                                <option value="-1">All</option>
                                {Object.keys(RECEIVESTATUS).map((key) =>
                                    !isNaN(key) ? (
                                        <option key={key} value={key}>
                                            {RECEIVESTATUS[key]}
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
                        labelL="Receiving Site"
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
                                {Object.keys(RECEIVESTATUS).map((key) =>
                                    !isNaN(key) ? (
                                        <option key={key} value={key}>
                                            {RECEIVESTATUS[key]} By
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
                        labelL="Item Type"
                        valueL={
                            <Form.Select
                                name="itemType"
                                value={formValues["itemType"]}
                                onChange={valueChanged}
                            >
                                <option value="-1">All Items</option>
                                <option value={ITEMTYPE.MATERIAL}>Materials</option>
                                <option value={ITEMTYPE.EQUIPMENT}>Equipments</option>
                            </Form.Select>
                        }
                        labelR={formValues["itemType"] === ITEMTYPE.MATERIAL.toString() && "Item"}
                        valueR={
                            formValues["itemType"] === ITEMTYPE.MATERIAL.toString() ? (
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
                            ) : null
                        }
                    />

                    {formValues["itemType"] === ITEMTYPE.EQUIPMENT.toString() && (
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
                    )}
                    <FormRow
                        labelL="Group By"
                        valueL={
                            <Form.Select
                                name="groupBy"
                                value={formValues["groupBy"]}
                                onChange={valueChanged}
                            >
                                {Object.keys(RECEIVEREPORTGROUPBY).map((i) => (
                                    <option key={i} value={RECEIVEREPORTGROUPBY[i].key}>
                                        {RECEIVEREPORTGROUPBY[i].label}
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
                            <h1 className="text-center mb-2 fs-4">Receive Report Summary</h1>
                            <BootstrapTable responsive striped bordered hover>
                                <thead>
                                    <tr>
                                        <th>#</th>
                                        <th>
                                            {
                                                RECEIVEREPORTGROUPBY[
                                                    Object.keys(RECEIVEREPORTGROUPBY).find(
                                                        (i) =>
                                                            RECEIVEREPORTGROUPBY[i].key ===
                                                            formValues.groupBy
                                                    )
                                                ]?.label
                                            }
                                        </th>
                                        <th>Number of Receives</th>
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

export default ReceiveReport;
