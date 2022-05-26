import { useState, useMemo, useEffect, useCallback, useRef } from "react";
import { Form, Button, Container, Table as BootstrapTable } from "react-bootstrap";

import LoadingSpinner from "../../fragments/LoadingSpinner";
import ConnectionError from "../../fragments/ConnectionError";
import FormRow from "../../fragments/FormRow";
import { useQuery, useQueryClient } from "react-query";
import { fetchSites } from "../../../api/site";
import {
    ITEMTYPE,
    GENERALREPORTGROUPBY,
    GENERALREPORTSELECTION,
    CHARTDATATYPE,
} from "../../../Constants";
import { fetchEquipmentCategories, fetchEquipmentCategory } from "../../../api/category";
import { fetchGeneralReport } from "../../../api/report";
import { fetchEmployees } from "../../../api/employee";
import { fetchMaterials } from "../../../api/item";
import Chart from "../../fragments/Chart";
import Select from "react-select";
import SimpleReportTable from "./fragments/SimpleReportTable";

function initFormValues() {
    return {
        fromDate: "",
        toDate: "",
        siteId: "-1",
        itemType: "-1",
        equipmentCategoryId: "-1",
        itemId: "-1",
        employeeId: "-1",
        groupBy: GENERALREPORTGROUPBY.ITEM.key,
        include: [GENERALREPORTSELECTION.INSTOCK],
    };
}

function GeneralReport() {
    const [formValues, setFormValues] = useState(initFormValues);
    const [chartData, setChartData] = useState();
    const [chartType, setChartType] = useState("bar");
    const [chartDataType, setChartDataType] = useState(CHARTDATATYPE.QTY.key);
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
    } = useQuery(
        ["general", formValues],
        () =>
            fetchGeneralReport({ ...formValues, include: formValues.include.map((x) => x.value) }),
        {
            enabled: false,
        }
    );

    function valueChanged(e) {
        queryClient.removeQueries(["general", formValues]);
        setFormValues({ ...formValues, [e.target.name]: e.target.value });
        setChartData(null);
    }

    const setChartDataCallback = useCallback(
        (summary) => {
            const data = {
                labels: summary.keys.map((key) => summary.labels[key]),
                datasets: [],
            };

            summary.stockSummary &&
                data.datasets.push({
                    label: "In Stock",
                    data: summary.keys.map(
                        (key) => summary.stockSummary[key]?.[chartDataType] ?? 0
                    ),
                });

            summary.minStockSummary &&
                data.datasets.push({
                    label: "Minimum Stock",
                    data: summary.keys.map(
                        (key) => summary.minStockSummary[key]?.[chartDataType] ?? 0
                    ),
                });

            summary.purchaseSummary &&
                data.datasets.push({
                    label: "Purchased",
                    data: summary.keys.map(
                        (key) => summary.purchaseSummary[key]?.[chartDataType] ?? 0
                    ),
                });

            summary.receiveSummary &&
                data.datasets.push({
                    label: "Received",
                    data: summary.keys.map(
                        (key) => summary.receiveSummary[key]?.[chartDataType] ?? 0
                    ),
                });

            summary.issueSummary &&
                data.datasets.push({
                    label: "Issued",
                    data: summary.keys.map(
                        (key) => summary.issueSummary[key]?.[chartDataType] ?? 0
                    ),
                });

            summary.borrowSummary &&
                data.datasets.push({
                    label: "Borrowed",
                    data: summary.keys.map(
                        (key) => summary.borrowSummary[key]?.[chartDataType] ?? 0
                    ),
                });

            summary.returnSummary &&
                data.datasets.push({
                    label: "Returned",
                    data: summary.keys.map(
                        (key) => summary.returnSummary[key]?.[chartDataType] ?? 0
                    ),
                });

            summary.damageSummary &&
                data.datasets.push({
                    label: "Damaged",
                    data: summary.keys.map(
                        (key) => summary.damageSummary[key]?.[chartDataType] ?? 0
                    ),
                });

            summary.transferInSummary &&
                data.datasets.push({
                    label: "Transferred In",
                    data: summary.keys.map(
                        (key) => summary.transferInSummary[key]?.[chartDataType] ?? 0
                    ),
                });

            summary.transferOutSummary &&
                data.datasets.push({
                    label: "Transferred Out",
                    data: summary.keys.map(
                        (key) => summary.transferOutSummary[key]?.[chartDataType] ?? 0
                    ),
                });

            //setTableData(summary);
            setChartData(data);
        },
        [formValues.groupBy, chartDataType]
    );

    useEffect(() => {
        if (queryResult === undefined) return;

        setChartDataCallback(queryResult);
    }, [queryResult, chartDataType]);

    return (
        <>
            <Container className="my-3">
                <Form className="mb-3">
                    <FormRow
                        labelL="Draw Chart"
                        valueL={
                            <Form.Select
                                value={chartDataType}
                                onChange={(e) => setChartDataType(e.target.value)}
                            >
                                {Object.keys(CHARTDATATYPE).map((i) => (
                                    <option key={CHARTDATATYPE[i].key} value={CHARTDATATYPE[i].key}>
                                        {CHARTDATATYPE[i].label}
                                    </option>
                                ))}
                            </Form.Select>
                        }
                        labelR="Include Items"
                        valueR={
                            <Select
                                placeholder="Select Items To Include"
                                style={{ display: "inline" }}
                                value={formValues.include}
                                onChange={(o) =>
                                    valueChanged({ target: { name: "include", value: o } })
                                }
                                className="asset-select"
                                closeMenuOnSelect={false}
                                isMulti
                                options={Object.values(GENERALREPORTSELECTION).map(
                                    (current) => current
                                )}
                            />
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
                                {Object.keys(GENERALREPORTGROUPBY).map((i) => (
                                    <option key={i} value={GENERALREPORTGROUPBY[i].key}>
                                        {GENERALREPORTGROUPBY[i].label}
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

                {(isLoading || isFetching) && <LoadingSpinner />}

                {isError && <ConnectionError status={error?.response?.status} />}

                {chartData && (
                    <Container fluid className="px-4">
                        <h1 className="display-6 text-center mb-2 ">Report Result</h1>
                        <Chart data={chartData} type={chartType} />
                    </Container>
                )}

                {queryResult?.stockSummary && (
                    <SimpleReportTable
                        title="In Stock Summary"
                        groupBy={formValues.groupBy}
                        data={queryResult.stockSummary}
                    />
                )}

                {queryResult?.minStockSummary && (
                    <SimpleReportTable
                        title="Minimum Stock Summary"
                        groupBy={formValues.groupBy}
                        data={queryResult.minStockSummary}
                    />
                )}

                {queryResult?.purchaseSummary && (
                    <SimpleReportTable
                        title="Purchased Items Summary"
                        groupBy={formValues.groupBy}
                        data={queryResult.purchaseSummary}
                    />
                )}

                {queryResult?.receiveSummary && (
                    <SimpleReportTable
                        title="Received Items Summary"
                        groupBy={formValues.groupBy}
                        data={queryResult.receiveSummary}
                    />
                )}

                {queryResult?.issueSummary && (
                    <SimpleReportTable
                        title="Issued Items Summary"
                        groupBy={formValues.groupBy}
                        data={queryResult.issueSummary}
                    />
                )}

                {queryResult?.borrowSummary && (
                    <SimpleReportTable
                        title="Borrowed Items Summary"
                        groupBy={formValues.groupBy}
                        data={queryResult.borrowSummary}
                    />
                )}

                {queryResult?.returnSummary && (
                    <SimpleReportTable
                        title="Returned Items Summary"
                        groupBy={formValues.groupBy}
                        data={queryResult.returnSummary}
                    />
                )}

                {queryResult?.damageSummary && (
                    <SimpleReportTable
                        title="Damaged Items Expense Summary"
                        groupBy={formValues.groupBy}
                        data={queryResult.damageSummary}
                    />
                )}

                {queryResult?.transferInSummary && (
                    <SimpleReportTable
                        title="Transferred In Items Summary"
                        groupBy={formValues.groupBy}
                        data={queryResult.transferInSummary}
                    />
                )}

                {queryResult?.transferOutSummary && (
                    <SimpleReportTable
                        title="Transferred Out Items Summary"
                        groupBy={formValues.groupBy}
                        data={queryResult.transferOutSummary}
                    />
                )}
            </Container>
        </>
    );
}

export default GeneralReport;
