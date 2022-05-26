import { useEffect, useRef, useState } from "react";
import { Form, Button, Container } from "react-bootstrap";
import Item from "../../../models/Item";
import ReturnReturnEquipment from "./fragments/ReturnReturnEquipment";
import Header from "../../layouts/Header";
import { useNavigate } from "react-router-dom";
import { useQuery, useMutation } from "react-query";
import { fetchSites } from "../../../api/site";
import { returnReturn } from "../../../api/return";
import LoadingSpinner from "../../fragments/LoadingSpinner";
import ConnectionError from "../../fragments/ConnectionError";
import { FaPlus } from "react-icons/fa";
import { fetchEmployees } from "../../../api/employee";
import { uploadFileAPI } from "../../../api/file";

function NewReturn() {
    const navigate = useNavigate();
    const [returnSite, setReturnSite] = useState(0);
    const [requestedBy, setRequestedBy] = useState(0);
    const [addedItems, setAddedItems] = useState([new Item()]);

    const [isLoading, setIsLoading] = useState(true);
    const [isError, setIsError] = useState(false);
    const [isUploadError, setIsUploadError] = useState(false);

    const sitesQuery = useQuery("sites", fetchSites, {
        onSuccess: (data) => {
            setReturnSite(data[0].siteId);
        },
    });

    const employeesQuery = useQuery(
        ["employees", returnSite],
        () => returnSite && fetchEmployees(returnSite)
    );

    useEffect(() => {
        setIsLoading(sitesQuery.isLoading || employeesQuery.isLoading);
    }, [sitesQuery.isLoading, employeesQuery.isLoading]);

    useEffect(() => {
        setIsError(isUploadError || sitesQuery.isError);
    }, [isUploadError, sitesQuery.isError]);

    const {
        isError: isSubmitError,
        isLoading: isSubmitLoading,
        error: submitError,
        mutate: submitReturnReturn,
    } = useMutation(returnReturn, {
        onSuccess: (res) => navigate(`/return/${res.data}`),
    });

    async function uploadFile(file) {
        const data = new FormData();
        data.append("file", file);

        try {
            return await uploadFileAPI(data);
        } catch {
            setIsUploadError(true);
        }
    }

    async function submit(e) {
        e.preventDefault();
        if (isSubmitLoading) return;

        var data = {
            siteId: returnSite,
            requestedById: requestedBy,
            borrowAssets: [],
        };

        for (var item of addedItems) {
            var tempItem = {
                itemId: item.itemId,
                equipmentModelId: item.equipmentModelId,
                equipmentAssetId: item.equipmentAssets[0].equipmentAssetId,
                assetDamageId: item.assetDamageId,
                returnRemark: item.returnRemark,
            };

            if (item.file !== null) tempItem.fileName = await uploadFile(item.file);

            data.borrowAssets.push(tempItem);
        }

        submitReturnReturn(data);
    }

    //scroll when Item is added
    var pageend = useRef(null);
    const [scroll, setScroll] = useState(false);

    useEffect(() => {
        if (scroll) {
            pageend.scrollIntoView();
            setScroll(false);
        }
    }, [scroll]);

    function addItem() {
        setAddedItems([...addedItems, new Item()]);
        setScroll(true);
    }

    if (isError || isSubmitError)
        return (
            <ConnectionError
                status={sitesQuery?.error?.response?.status ?? submitError?.response?.status}
            />
        );

    if (isLoading) return <LoadingSpinner />;

    return (
        <>
            <Header title="Return Borrowed Assets" />

            <Container className="my-3">
                <Form onSubmit={submit}>
                    <div className="row mx-2">
                        <div className="col">
                            <Form.Group className="mb-3">
                                <div className="row">
                                    <div className="col-1 mt-1" style={{ width: "15%" }}>
                                        <Form.Label>Borrowed By</Form.Label>
                                    </div>
                                    <div className="col">
                                        <Form.Select
                                            required
                                            value={requestedBy}
                                            onChange={(e) => setRequestedBy(e.target.value)}
                                        >
                                            <option value="">Select Employee</option>
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
                                    </div>
                                </div>
                            </Form.Group>
                        </div>
                    </div>

                    {addedItems.map((item, index) => (
                        <ReturnReturnEquipment
                            key={item.key}
                            employee={Number(requestedBy)}
                            index={index}
                            addedItems={addedItems}
                            setAddedItems={setAddedItems}
                        />
                    ))}

                    <div className="row">
                        <div className="col-9 d-grid">
                            <Button type="submit" className="btn-teal-dark">
                                Return Assets
                            </Button>
                        </div>
                        <div className="col-3 d-grid">
                            <Button className="btn-teal" onClick={addItem}>
                                <FaPlus className="me-2 mb-1" /> Add Item
                            </Button>
                        </div>
                    </div>
                </Form>
                <div
                    style={{ float: "left", clear: "both" }}
                    ref={(el) => {
                        pageend = el;
                    }}
                ></div>
            </Container>
        </>
    );
}

export default NewReturn;
