import { useEffect, useRef, useState } from "react";
import { Form, Button, Container } from "react-bootstrap";
import Item from "../../../models/Item";
import MaintenanceRequestEquipment from "./fragments/MaintenanceRequestEquipment";
import Header from "../../layouts/Header";
import { useNavigate } from "react-router-dom";
import { useQuery, useMutation } from "react-query";
import { fetchSites } from "../../../api/site";
import { requestMaintenance } from "../../../api/maintenance";
import LoadingSpinner from "../../fragments/LoadingSpinner";
import ConnectionError from "../../fragments/ConnectionError";

function NewMaintenance() {
    const navigate = useNavigate();
    const [sendSite, setSendSite] = useState(0);
    const [addedItems, setAddedItems] = useState([new Item()]);

    const {
        isError: isSubmitError,
        isLoading: isSubmitLoading,
        error: submitError,
        mutate: submitMaintenanceRequest,
    } = useMutation(requestMaintenance, {
        onSuccess: (res) => navigate(`/maintenance/${res.data}`),
    });

    function submit(e) {
        e.preventDefault();
        if (isSubmitLoading) return;

        var data = {
            siteId: sendSite,
            itemId: addedItems[0].itemId,
            equipmentModelId: addedItems[0].equipmentModelId,
            reason: addedItems[0].reason,
        };

        if (addedItems[0].equipmentAssets[0].equipmentAssetId)
            data.equipmentAssetId = addedItems[0].equipmentAssets[0].equipmentAssetId;

        submitMaintenanceRequest(data);
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


    if (isSubmitError)
        return (
            <ConnectionError
                status={submitError?.response?.status ?? sitesQuery?.error?.response?.status}
            />
        );

    return (
        <>
            <Header title="New Maintenance Request" />

            <Container className="my-3">
                <Form onSubmit={submit}>
                    {addedItems.map((item, index) => (
                        <MaintenanceRequestEquipment
                            key={item.key}
                            index={index}
                            addedItems={addedItems}
                            setAddedItems={setAddedItems}
                        />
                    ))}

                    <div className="row">
                        <div className="col d-grid">
                            <Button type="submit" className="btn-teal-dark">
                                Request Maintenance
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

export default NewMaintenance;
