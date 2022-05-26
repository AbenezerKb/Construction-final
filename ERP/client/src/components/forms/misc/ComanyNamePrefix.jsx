import { useEffect, useState } from "react";
import Header from "../../layouts/Header";
import { Container, Form, Row, Col, Button, Spinner } from "react-bootstrap";
import { ToastContainer, toast } from "react-toastify";
import { useQuery, useMutation, useQueryClient } from "react-query";
import { updateCompanyNamePrefix, fetchCompanyNamePrefix } from "../../../api/misc";
import LoadingSpinner from "../../fragments/LoadingSpinner";
import ConnectionError from "../../fragments/ConnectionError";

function ComanyNamePrefix() {
    const [formValues, setFormValues] = useState({ name: "", prefix: "" });
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
        data: queryResultData,
        isLoading,
        isFetching,
        isError,
        error,
    } = useQuery(["company"], () => fetchCompanyNamePrefix());

    const {
        isLoading: isSubmitLoading,
        isError: isSubmitError,
        error: submitError,
        mutate: submit,
    } = useMutation(updateCompanyNamePrefix, {
        onSuccess: (res) => {
            queryClient.invalidateQueries("company");
            toast.success("Company Name and Prefix Updated Succcessfully", toastOption);
        },
    });

    useEffect(() => {
        if (queryResultData == undefined) return;
        setFormValues(queryResultData);
    }, [queryResultData]);

    function submitUpdate(e) {
        e.preventDefault();
        if (isSubmitLoading) return;

        submit(formValues);
    }

    function valueChanged(e) {
        setFormValues({ ...formValues, [e.target.name]: e.target.value });
    }

    return (
        <>
            <Header title="Company Name and Asset Number Prefix" />
            <Container className="my-3 align-self-center">
                <Form onSubmit={submitUpdate}>
                    <div className="col-10 mx-auto shadow py-5 px-5 rounded">
                        <div className="row ">
                            <div className="col">
                                <Form.Group className="mb-3">
                                    <Form.Label>Company Name</Form.Label>
                                    <Form.Control
                                        type="text"
                                        name="name"
                                        value={formValues["name"]}
                                        onChange={valueChanged}
                                        required
                                    />
                                </Form.Group>
                            </div>
                        </div>
                        <div className="row ">
                            <div className="col">
                                <Form.Group className="mb-3">
                                    <Form.Label>Asset Number Prefix</Form.Label>
                                    <Form.Control
                                        type="text"
                                        name="prefix"
                                        value={formValues["prefix"]}
                                        onChange={valueChanged}
                                        required
                                    />
                                </Form.Group>
                            </div>
                        </div>
                        <Row>
                            <Col className="mt-3 d-grid">
                                <Button className="btn-teal-dark" type="submit">
                                    {isSubmitLoading && (
                                        <Spinner className="me-2" animation="grow" size="sm" />
                                    )}
                                    Update
                                </Button>
                            </Col>
                        </Row>
                    </div>
                </Form>
                <ToastContainer />
            </Container>
        </>
    );
}

export default ComanyNamePrefix;
