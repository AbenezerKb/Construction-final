import Header from "../../layouts/Header";
import Role from "../../../models/Role";
import { useState, useMemo } from "react";
import { Form, Button, Container, InputGroup } from "react-bootstrap";
import { FaPlus } from "react-icons/fa";
import ToggleBox from "../../layouts/ToggleBox";
import { ToastContainer, toast } from "react-toastify";
import { useMutation } from "react-query";
import { addRole } from "../../../api/role";
import LoadingSpinner from "../../fragments/LoadingSpinner";
import ConnectionError from "../../fragments/ConnectionError";
import AlertNotification from "../../fragments/AlertNotification";

function NewRole() {
    const [role, setRole] = useState([new Role()]);

    const [isLoading, setIsLoading] = useState(false);
    const [isError, setIsError] = useState(false);

    const toastOption = {
        position: "bottom-right",
        autoClose: 5000,
        hideProgressBar: false,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true,
        theme: "dark",
        // progress: undefined
    };

    const {
        isError: isSubmitError,
        isLoading: isSubmitLoading,
        mutate: submitNewRole,
    } = useMutation(addRole, {
        onSuccess: (res) => {
            toast.success("Role Is Successfully Added", toastOption);
        },
    });

    function submit(e) {
        e.preventDefault();
        toast.success("Role Is Successfully Added", toastOption);
    }

    if (isError || isSubmitError) return <ConnectionError />;

    return (
        <>
            <Header title="Add New Role" />

            <Container className="my-3">
                <AlertNotification
                    title="Feature Under Construction"
                    content="Adding Role Feature is Under Construction. It Will Be Available Once Development Is Completed."
                />
                <Form onSubmit={submit}>
                    <div>
                        <div className="row">
                            <div className="mx-4 col">
                                <Form.Group>
                                    <InputGroup className="mb-3">
                                        <Form.Label className="col-3 pt-2 h5">
                                            Role Name
                                        </Form.Label>
                                        <Form.Control type="text" />
                                    </InputGroup>
                                </Form.Group>
                            </div>
                        </div>

                        <div className="row py-2 text-center">
                            <h4>Set User Permissions</h4>
                        </div>
                        <div className="my-2">
                            <h5 className="px-4">User Management</h5>
                            <div className="row mx-4">
                                <div className="col-6">
                                    <Form.Group className="mb-3">
                                        <Form.Check
                                            inline
                                            label="Can Approve User Acconunt"
                                            value="1"
                                            type="checkbox"
                                        ></Form.Check>
                                    </Form.Group>
                                </div>
                                <div className="col-6">
                                    <Form.Group className="mb-3">
                                        <Form.Check
                                            inline
                                            label="Can Delete User Account"
                                            name="bool1"
                                            type="checkbox"
                                        ></Form.Check>
                                    </Form.Group>
                                </div>
                            </div>
                        </div>
                        <div className="my-4">
                            <h5 className="px-4">Purchase</h5>
                            <div className="row mx-4">
                                <div className="col-6">
                                    <Form.Group className="mb-3">
                                        <Form.Check
                                            inline
                                            label="Can View Purchase Requests"
                                            name="bool"
                                            type="checkbox"
                                        ></Form.Check>
                                    </Form.Group>
                                </div>
                                <div className="col-6">
                                    <Form.Group className="mb-3">
                                        <Form.Check
                                            inline
                                            label="Can Request Purchase"
                                            name="bool1"
                                            type="checkbox"
                                        ></Form.Check>
                                    </Form.Group>
                                </div>
                            </div>
                            <div className="row mx-4">
                                <div className="col-6">
                                    <Form.Group className="mb-3">
                                        <Form.Check
                                            inline
                                            label="Can Check Purchase"
                                            name="bool"
                                            type="checkbox"
                                        ></Form.Check>
                                    </Form.Group>
                                </div>
                                <div className="col-6">
                                    <Form.Group className="mb-3">
                                        <Form.Check
                                            inline
                                            label="Can Approve Purchase"
                                            name="bool1"
                                            type="checkbox"
                                        ></Form.Check>
                                    </Form.Group>
                                </div>
                            </div>
                            <div className="row mx-4">
                                <div className="col-6">
                                    <Form.Group className="mb-3">
                                        <Form.Check
                                            inline
                                            label="Can Confirm Purchase"
                                            name="bool"
                                            type="checkbox"
                                        ></Form.Check>
                                    </Form.Group>
                                </div>
                            </div>
                        </div>
                        <div className="my-4">
                            <h5 className="px-4">Buy</h5>
                            <div className="row mx-4">
                                <div className="col-6">
                                    <Form.Group className="mb-3">
                                        <Form.Check
                                            inline
                                            label="Can View Buy Requests"
                                            name="bool"
                                            type="checkbox"
                                        ></Form.Check>
                                    </Form.Group>
                                </div>
                                <div className="col-6">
                                    <Form.Group className="mb-3">
                                        <Form.Check
                                            inline
                                            label="Can Request Buy"
                                            name="bool1"
                                            type="checkbox"
                                        ></Form.Check>
                                    </Form.Group>
                                </div>
                            </div>
                            <div className="row mx-4">
                                <div className="col-6">
                                    <Form.Group className="mb-3">
                                        <Form.Check
                                            inline
                                            label="Can Check Buy"
                                            name="bool"
                                            type="checkbox"
                                        ></Form.Check>
                                    </Form.Group>
                                </div>
                                <div className="col-6">
                                    <Form.Group className="mb-3">
                                        <Form.Check
                                            inline
                                            label="Can Approve Buy"
                                            name="bool1"
                                            type="checkbox"
                                        ></Form.Check>
                                    </Form.Group>
                                </div>
                            </div>
                            <div className="row mx-4">
                                <div className="col-6">
                                    <Form.Group className="mb-3">
                                        <Form.Check
                                            inline
                                            label="Can Confirm Buy"
                                            name="bool"
                                            type="checkbox"
                                        ></Form.Check>
                                    </Form.Group>
                                </div>
                            </div>
                        </div>
                        <div className="my-4">
                            <h5 className="px-4">Receive</h5>
                            <div className="row mx-4">
                                <div className="col-6">
                                    <Form.Group className="mb-3">
                                        <Form.Check
                                            inline
                                            label="Can View Receive Requests"
                                            name="bool"
                                            type="checkbox"
                                        ></Form.Check>
                                    </Form.Group>
                                </div>
                                <div className="col-6">
                                    <Form.Group className="mb-3">
                                        <Form.Check
                                            inline
                                            label="Can Receive Item"
                                            name="bool1"
                                            type="checkbox"
                                        ></Form.Check>
                                    </Form.Group>
                                </div>
                            </div>
                            <div className="row mx-4">
                                <div className="col-6">
                                    <Form.Group className="mb-3">
                                        <Form.Check
                                            inline
                                            label="Can Approve Receive"
                                            name="bool"
                                            type="checkbox"
                                        ></Form.Check>
                                    </Form.Group>
                                </div>
                            </div>
                        </div>
                        <div className="my-4">
                            <h5 className="px-4">Issue</h5>
                            <div className="row mx-4">
                                <div className="col-6">
                                    <Form.Group className="mb-3">
                                        <Form.Check
                                            inline
                                            label="Can View Issue Requests"
                                            name="bool"
                                            type="checkbox"
                                        ></Form.Check>
                                    </Form.Group>
                                </div>
                                <div className="col-6">
                                    <Form.Group className="mb-3">
                                        <Form.Check
                                            inline
                                            label="Can Request An Issue"
                                            name="bool1"
                                            type="checkbox"
                                        ></Form.Check>
                                    </Form.Group>
                                </div>
                            </div>
                            <div className="row mx-4">
                                <div className="col-6">
                                    <Form.Group className="mb-3">
                                        <Form.Check
                                            inline
                                            label="Can Approve An Issue Buy"
                                            name="bool"
                                            type="checkbox"
                                        ></Form.Check>
                                    </Form.Group>
                                </div>
                                <div className="col-6">
                                    <Form.Group className="mb-3">
                                        <Form.Check
                                            inline
                                            label="Can Hand An Issue"
                                            name="bool1"
                                            type="checkbox"
                                        ></Form.Check>
                                    </Form.Group>
                                </div>
                            </div>
                        </div>
                        <div className="my-4">
                            <h5 className="px-4">Borrow</h5>
                            <div className="row mx-4">
                                <div className="col-6">
                                    <Form.Group className="mb-3">
                                        <Form.Check
                                            inline
                                            label="Can View Borrow Requests"
                                            name="bool"
                                            type="checkbox"
                                        ></Form.Check>
                                    </Form.Group>
                                </div>
                                <div className="col-6">
                                    <Form.Group className="mb-3">
                                        <Form.Check
                                            inline
                                            label="Can Request Borrow"
                                            name="bool1"
                                            type="checkbox"
                                        ></Form.Check>
                                    </Form.Group>
                                </div>
                            </div>
                            <div className="row mx-4">
                                <div className="col-6">
                                    <Form.Group className="mb-3">
                                        <Form.Check
                                            inline
                                            label="Can Approve Borrow"
                                            name="bool"
                                            type="checkbox"
                                        ></Form.Check>
                                    </Form.Group>
                                </div>
                                <div className="col-6">
                                    <Form.Group className="mb-3">
                                        <Form.Check
                                            inline
                                            label="Can Hand Borrow"
                                            name="bool1"
                                            type="checkbox"
                                        ></Form.Check>
                                    </Form.Group>
                                </div>
                            </div>
                            <div className="row mx-4">
                                <div className="col-6">
                                    <Form.Group className="mb-3">
                                        <Form.Check
                                            inline
                                            label="Can Return Borrow"
                                            name="bool"
                                            type="checkbox"
                                        ></Form.Check>
                                    </Form.Group>
                                </div>
                            </div>
                        </div>
                        <div className="my-4">
                            <h5 className="px-4">Maintenance</h5>
                            <div className="row mx-4">
                                <div className="col-6">
                                    <Form.Group className="mb-3">
                                        <Form.Check
                                            inline
                                            label="Can View Maintenance Requests"
                                            name="bool"
                                            type="checkbox"
                                        ></Form.Check>
                                    </Form.Group>
                                </div>
                                <div className="col-6">
                                    <Form.Group className="mb-3">
                                        <Form.Check
                                            inline
                                            label="Can Request Maintenance"
                                            name="bool1"
                                            type="checkbox"
                                        ></Form.Check>
                                    </Form.Group>
                                </div>
                            </div>
                            <div className="row mx-4">
                                <div className="col-6">
                                    <Form.Group className="mb-3">
                                        <Form.Check
                                            inline
                                            label="Can Fix Maintenance"
                                            name="bool"
                                            type="checkbox"
                                        ></Form.Check>
                                    </Form.Group>
                                </div>
                                <div className="col-6">
                                    <Form.Group className="mb-3">
                                        <Form.Check
                                            inline
                                            label="Can Approve Maintenance"
                                            name="bool1"
                                            type="checkbox"
                                        ></Form.Check>
                                    </Form.Group>
                                </div>
                            </div>
                        </div>
                        <div className="my-4">
                            <h5 className="px-4">Notification</h5>
                            <div className="row mx-4">
                                <div className="col-6">
                                    <Form.Group className="mb-3">
                                        <Form.Check
                                            inline
                                            label="Can Get Stock Notification"
                                            name="bool"
                                            type="checkbox"
                                        ></Form.Check>
                                    </Form.Group>
                                </div>
                            </div>
                        </div>
                        <div className="row mx-4">
                            <div className="d-grid">
                                <Button type="submit" className="btn-teal">
                                    <FaPlus className="me-2 mb-1" /> Add Role
                                </Button>
                            </div>
                        </div>
                    </div>
                </Form>
                <ToastContainer />
            </Container>
        </>
    );
}

export default NewRole;
