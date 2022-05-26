import { useEffect, useRef, useState } from "react";
import { Form, Button, Container } from "react-bootstrap";
import { FaPlus } from "react-icons/fa";
import Item from "../../../models/Item";
import BuyRequestMaterial from "./fragments/BuyRequestMaterial";
import Header from "../../layouts/Header";
import { useNavigate } from "react-router-dom";
import { useQuery, useMutation } from "react-query";
import { fetchSites } from "../../../api/site";
import { fetchMaterials } from "../../../api/item";
import { requestBuy} from "../../../api/buy";
import LoadingSpinner from "../../fragments/LoadingSpinner";
import ConnectionError from "../../fragments/ConnectionError";

function MaterialBuy() {
    const navigate = useNavigate();
    const [addedItems, setAddedItems] = useState([new Item()]);

    const [allSites, setAllSites] = useState([]);
    const [allItems, setAllItems] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [isError, setIsError] = useState(false);

    const sitesQuery = useQuery("sites", fetchSites, {
        onSuccess: (data) => {
            setAllSites(data);
        },
    });

    const itemQuery = useQuery("materials", fetchMaterials, {
        onSuccess: (data) => setAllItems(data),
    });

    useEffect(() => {
        setIsLoading(sitesQuery.isLoading || itemQuery.isLoading);
    }, [sitesQuery.isLoading, itemQuery.isLoading]);

    useEffect(() => {
        setIsError(sitesQuery.isError || itemQuery.isError);
    }, [sitesQuery.isError, itemQuery.isError]);

    const {
        isError: isSubmitError,
        isLoading: isSubmitLoading,
        mutate: submitBuyRequest,
    } = useMutation(requestBuy, {
        onSuccess: (res) => navigate(`/buy/${res.data}`),
    });

    function submit(e) {
        e.preventDefault();
        if (isSubmitLoading) return;

        var data = {
            buyItems: [],
        };

        for (let item of addedItems) {
            let tempItem = {
                itemId: item.itemId,
                qtyRequested: item.qtyRequested,
                requestRemark: item.requestRemark,
            };

            data.buyItems.push(tempItem);
        }

        submitBuyRequest(data);
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

    if (isLoading) return <LoadingSpinner />;

    if (isError || isSubmitError) return <ConnectionError />;

    return (
        <>
            <Header title="New Material Buy Request" />

            <Container className="my-3">
                <Form onSubmit={submit}>

                    {addedItems.map((item, index) => (
                        <BuyRequestMaterial
                            key={item.key}
                            index={index}
                            allItems={allItems}
                            addedItems={addedItems}
                            setAddedItems={setAddedItems}
                        />
                    ))}

                    <div className="row">
                        <div className="col-9 d-grid">
                            <Button type="submit" className="btn-teal-dark">
                                Request Buy
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

export default MaterialBuy;
