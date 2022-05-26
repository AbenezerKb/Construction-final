import { useEffect, useRef, useState } from "react";
import { Form, Button, Container } from "react-bootstrap";
import { FaPlus } from "react-icons/fa";
import Item from "../../../models/Item";
import ProductionRequestEquipment from "./fragments/ProductionRequestEquipment";
import Header from "../../layouts/Header";
import { useNavigate } from "react-router-dom";
import { useQuery, useMutation } from "react-query";
import { fetchSites } from "../../../api/site";
import { requestBorrow } from "../../../api/borrow";
import LoadingSpinner from "../../fragments/LoadingSpinner";
import ConnectionError from "../../fragments/ConnectionError";
import AlertNotification from "../../fragments/AlertNotification";

function NewProduction() {
    const navigate = useNavigate();
    const [addedItems, setAddedItems] = useState([new Item()]);

    function submit(e) {
        e.preventDefault();
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

    return (
        <>
            <Header title="New Equipment Production Request" />

            <Container className="my-3">
                <AlertNotification
                    title="Feature Under Construction"
                    content="An Equipment Production Feature is Under Construction. It Will Be Available Once Development Is Completed."
                />
                <Form onSubmit={submit}>
                    {addedItems.map((item, index) => (
                        <ProductionRequestEquipment
                            key={item.key}
                            index={index}
                            addedItems={addedItems}
                            setAddedItems={setAddedItems}
                        />
                    ))}

                    <div className="row">
                        <div className="col-9 d-grid">
                            <Button type="submit" className="btn-teal-dark">
                                Request Production
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

export default NewProduction;
