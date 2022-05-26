import { useEffect, useRef, useState } from "react";
import { Form, Button, Container } from "react-bootstrap";
import { FaPlus } from "react-icons/fa";
import Item from "../../../models/Item";
import BorrowRequestEquipment from "./fragments/BorrowRequestEquipment";
import Header from "../../layouts/Header";
import { useNavigate } from "react-router-dom";
import { useQuery, useMutation } from "react-query";
import { fetchSites } from "../../../api/site";
import { requestBorrow } from "../../../api/borrow";
import LoadingSpinner from "../../fragments/LoadingSpinner";
import ConnectionError from "../../fragments/ConnectionError";
import BorrowCategorySelection from "./fragments/BorrowCategorySelection";

function NewBorrow() {
    const navigate = useNavigate();
    const [addedItems, setAddedItems] = useState([new Item()]);
    const [categoryId, setCategoryId] = useState(-1);

    const {
        isError: isSubmitError,
        error: submitError,
        isLoading: isSubmitLoading,
        mutate: submitBorrowRequest,
    } = useMutation(requestBorrow, {
        onSuccess: (res) => navigate(`/borrow/${res.data}`),
    });

    function submit(e) {
        e.preventDefault();
        if (isSubmitLoading) return;

        var data = {
            borrowItems: [],
        };

        for (let item of addedItems) {
            let tempItem = {
                itemId: item.itemId,
                equipmentModelId: item.equipmentModelId,
                qtyRequested: item.qtyRequested,
                requestRemark: item.requestRemark,
            };

            data.borrowItems.push(tempItem);
        }

        submitBorrowRequest(data);
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

    if (isSubmitError) return <ConnectionError status={submitError?.response?.status} />;

    return (
        <>
            <Header title="New Borrow Request" />

            {categoryId === -1 ? (
                <Container className="py-3">
                    <BorrowCategorySelection setCategoryId={setCategoryId} />
                </Container>
            ) : (
                <Container className="my-3">
                    <div className="d-grid">
                        <Button className="btn-teal mb-3" onClick={() => setCategoryId(-1)}>
                            Change Category
                        </Button>
                    </div>
                    <Form onSubmit={submit}>
                        {addedItems.map((item, index) => (
                            <BorrowRequestEquipment
                                key={item.key}
                                index={index}
                                addedItems={addedItems}
                                setAddedItems={setAddedItems}
                                categoryId={categoryId}
                            />
                        ))}

                        <div className="row">
                            <div className="col-9 d-grid">
                                <Button type="submit" className="btn-teal-dark">
                                    Request Borrow
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
            )}
        </>
    );
}

export default NewBorrow;
