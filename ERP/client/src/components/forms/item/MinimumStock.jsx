import { useEffect, useState } from "react";
import { Container, Row, Col, Form, Button, Spinner } from "react-bootstrap";
import Header from "../../layouts/Header";
import FormRow from "../../fragments/FormRow";
import { ITEMTYPE } from "../../../Constants";
import { useQuery, useMutation, useQueryClient } from "react-query";
import { fetchMaterials, fetchMinimumStockItems, updateMinimumStockItems } from "../../../api/item";
import { fetchEquipmentCategories, fetchEquipmentCategory } from "../../../api/category";
import { ToastContainer, toast } from "react-toastify";
import ConnectionError from "../../fragments/ConnectionError";
import LoadingSpinner from "../../fragments/LoadingSpinner";

function initFormValues() {
    return {
        itemType: "-1",
        equipmentCategoryId: "-1",
        itemId: "-1",
    };
}

function MinimumStock() {
    const [formValues, setFormValues] = useState(initFormValues);
    const [stockValues, setStockValues] = useState([]);
    const queryClient = useQueryClient();

    const toastOption = {
        position: "bottom-right",
        autoClose: 5000,
        hideProgressBar: false,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true,
        theme: "dark",
    };

    const materialsQuery = useQuery(["materials"], fetchMaterials);
    const categoriesQuery = useQuery("equipmentcategories", fetchEquipmentCategories);
    const equipmentsQuery = useQuery(
        ["equipmentcategory", formValues.equipmentCategoryId],
        () =>
            formValues.equipmentCategoryId !== "-1" &&
            formValues.equipmentCategoryId !== undefined &&
            fetchEquipmentCategory(formValues.equipmentCategoryId)
    );

    function valueChanged(e) {
        const data = { ...formValues };

        data[e.target.name] = e.target.value;

        if (e.target.name === "itemType") {
            data.equipmentCategoryId = "-1";
            data.itemId = "-1";
        }

        if (e.target.name === "equipmentCategoryId") data.itemId = "-1";

        setFormValues(data);
    }

    const {
        isLoading: isSubmitLoading,
        isError: isSubmitError,
        error: submitError,
        mutate: submit,
    } = useMutation(updateMinimumStockItems, {
        onSuccess: (res) => {
            toast.success("Minimum Stock Updated Succcessfully", toastOption);
            queryClient.invalidateQueries("minimumstock");
        },
    });

    function stockValueChanged(itemId, equipmentModelId, e) {
        setStockValues(
            stockValues.map((stockItem) =>
                stockItem.itemId === itemId && stockItem.equipmentModelId === equipmentModelId
                    ? { ...stockItem, qty: e.target.value }
                    : stockItem
            )
        );
    }

    function submitUpdate(e) {
        e.preventDefault();
        if (isSubmitLoading) return;

        submit(stockValues);
    }

    const {
        data: queryResultData,
        isLoading,
        isFetching,
        isError,
        error,
    } = useQuery(["minimumstock", formValues], () => fetchMinimumStockItems(formValues));

    useEffect(() => {
        if (queryResultData == undefined) return;

        setStockValues(queryResultData);
    }, [queryResultData]);

    return (
        <>
            <Header title="Minimum Stock" />

            <Container className="pt-3">
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

                {isError || isSubmitError ? (
                    <ConnectionError
                        status={error?.response?.status && submitError?.response?.status}
                    />
                ) : null}

                {isLoading || isFetching ? <LoadingSpinner /> : null}

                {stockValues.length ? (
                    <Row className="my-3 display-6 fs-3">
                        <div className="text-center"> Selected Items</div>
                    </Row>
                ) : null}

                <Form onSubmit={submitUpdate}>
                    <Row xs={1} lg={2} className="mx-2">
                        {stockValues?.map((minStockItem) => (
                            <Col
                                key={`${minStockItem.siteId}${minStockItem.itemId}${minStockItem.equipmentModelId}`}
                            >
                                <Form.Group className="mb-3">
                                    <Row>
                                        <Col>
                                            <Form.Label>{minStockItem.name}</Form.Label>
                                        </Col>
                                        <Col>
                                            <Form.Control
                                                type="number"
                                                value={minStockItem.qty}
                                                onChange={stockValueChanged.bind(
                                                    this,
                                                    minStockItem.itemId,
                                                    minStockItem.equipmentModelId
                                                )}
                                            />
                                        </Col>
                                    </Row>
                                </Form.Group>
                            </Col>
                        ))}
                    </Row>

                    {stockValues.length ? (
                        <Row>
                            <Col className=" d-grid">
                                <Button className="btn-teal-dark" type="submit">
                                    {isSubmitLoading && (
                                        <Spinner className="me-2" animation="grow" size="sm" />
                                    )}
                                    Update Minimum Stock
                                </Button>
                            </Col>
                        </Row>
                    ) : null}
                </Form>
                <ToastContainer />
            </Container>
        </>
    );
}

export default MinimumStock;
