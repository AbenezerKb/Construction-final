import { useEffect, useState, useRef } from "react";
import { Container, Row, Col, Form, Button, Spinner } from "react-bootstrap";
import Header from "../../layouts/Header";
import FormRow from "../../fragments/FormRow";
import { useQuery, useMutation, useQueryClient } from "react-query";
import { fetchDamages, addDamage, updateDamages } from "../../../api/damage";
import { ToastContainer, toast } from "react-toastify";
import ConnectionError from "../../fragments/ConnectionError";
import LoadingSpinner from "../../fragments/LoadingSpinner";

function ManageDamages() {
    const nameRef = useRef();
    const percentageRef = useRef();
    const [damageValues, setDamageValues] = useState([]);
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

    const {
        isLoading: isSubmitLoading,
        isError: isSubmitError,
        error: submitError,
        mutate: submit,
    } = useMutation(updateDamages, {
        onSuccess: (res) => {
            toast.success("Asset Damages Updated Succcessfully", toastOption);
            queryClient.invalidateQueries("damages");
        },
    });

    const {
        isLoading: isAddLoading,
        isError: isAddError,
        error: addError,
        mutate: add,
    } = useMutation(addDamage, {
        onSuccess: (res) => {
            nameRef.current.value = "";
            percentageRef.current.value = "";
            toast.success("Asset Damage Added Succcessfully", toastOption);
            queryClient.invalidateQueries("damages");
        },
    });

    function damageValueChanged(assetDamageId, e) {
        setDamageValues(
            damageValues.map((stockItem) =>
                stockItem.assetDamageId === assetDamageId
                    ? { ...stockItem, [e.target.name]: e.target.value }
                    : stockItem
            )
        );
    }

    function submitAdd(e) {
        e.preventDefault();
        if (isAddLoading) return;

        const data = {
            name: nameRef.current.value,
            penalityPercentage: percentageRef.current.value,
        };
        add(data);
    }

    function submitUpdate(e) {
        e.preventDefault();
        if (isSubmitLoading) return;

        submit(damageValues);
    }

    const {
        data: queryResultData,
        isLoading,
        isFetching,
        isError,
        error,
    } = useQuery(["damages"], () => fetchDamages());

    useEffect(() => {
        if (queryResultData == undefined) return;

        setDamageValues(queryResultData);
    }, [queryResultData]);

    return (
        <>
            <Header title="Manage Damages" />

            <Container className="pt-3">
                <h1 className="display-5 fs-4 ps-3 pb-3">Add Equipment Asset Damage</h1>
                <Form onSubmit={submitAdd}>
                    <FormRow
                        labelL="Name"
                        valueL={<Form.Control required ref={nameRef} />}
                        labelR="Penality Percentage"
                        valueR={
                            <Form.Control
                                required
                                ref={percentageRef}
                                type="number"
                                min="0"
                                max="1"
                                step=".01"
                            />
                        }
                    />
                    <div className="d-grid mb-4">
                        <Button className="btn-teal-dark" type="submit">
                            {isSubmitLoading && (
                                <Spinner className="me-2" animation="grow" size="sm" />
                            )}
                            Add Asset Damage
                        </Button>
                    </div>
                </Form>

                {isError || isSubmitError || isAddError ? (
                    <ConnectionError
                        status={
                            error?.response?.status &&
                            submitError?.response?.status &&
                            addError?.response?.status
                        }
                    />
                ) : null}

                {isLoading || isFetching ? <LoadingSpinner /> : null}

                {damageValues.length ? (
                    <Row className="my-3 display-6 fs-3">
                        <div className="text-center"> Selected Items</div>
                    </Row>
                ) : null}
                <Form onSubmit={submitUpdate}>
                    <Container>
                        {damageValues?.map((minStockItem) => (
                            <FormRow
                                key={`${minStockItem.assetDamageId}`}
                                labelL="Name"
                                valueL={
                                    <Form.Control
                                        required
                                        name="name"
                                        value={minStockItem.name}
                                        onChange={damageValueChanged.bind(
                                            this,
                                            minStockItem.assetDamageId
                                        )}
                                    />
                                }
                                labelR="Penality Percentage"
                                valueR={
                                    <Form.Control
                                        required
                                        type="number"
                                        min="0"
                                        max="1"
                                        step=".01"
                                        name="penalityPercentage"
                                        value={minStockItem.penalityPercentage}
                                        onChange={damageValueChanged.bind(
                                            this,
                                            minStockItem.assetDamageId
                                        )}
                                    />
                                }
                            />
                        ))}
                    </Container>

                    {damageValues.length ? (
                        <Row>
                            <Col className=" d-grid">
                                <Button className="btn-teal-dark" type="submit">
                                    {isSubmitLoading && (
                                        <Spinner className="me-2" animation="grow" size="sm" />
                                    )}
                                    Update Asset Damages
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

export default ManageDamages;
