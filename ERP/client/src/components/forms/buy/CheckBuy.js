import React from "react";
import PropTypes from "prop-types";
import BuyCheckMaterial from "./fragments/BuyCheckMaterial";
import { Button, Spinner } from "react-bootstrap";
import { checkBuy, queueBuy } from "../../../api/buy";
import { useMutation, useQueryClient } from "react-query";
import ConnectionError from "../../fragments/ConnectionError";

function CheckBuy({ addedItems, setAddedItems, buy }) {
    const queryClient = useQueryClient();

    const {
        isLoading: isCheckLoading,
        isError: isCheckError,
        mutate: submitCheckBuy,
    } = useMutation(checkBuy, {
        onSuccess: (res) =>
            queryClient.invalidateQueries(["buy", buy.buyId.toString()]),
    });

    const {
        isLoading: isQueueLoading,
        isError: isQueueError,
        mutate: submitQueueBuy,
    } = useMutation(queueBuy, {
        onSuccess: (res) =>
            queryClient.invalidateQueries(["buy", buy.buyId.toString()]),
    });


    function submit(type, e) {
        e.preventDefault();
        if (isCheckLoading || isQueueLoading) return;

        const data = {
            buyId: buy.buyId,
            buyItems: [],
        };

        for (let item of addedItems) {
            let tempItem = {
                itemId: Number(item.itemId),
                qtyApproved: Number(item.qtyApproved),
                approveRemark: item.approveRemark,
            };

            data.buyItems.push(tempItem);
        }

        if (type === "queue") submitQueueBuy(data);
        else submitCheckBuy(data);
    }

    if (isCheckError || isQueueError) return <ConnectionError />;

    return (
        <>
            {addedItems.map((item, index) => (
                <BuyCheckMaterial
                    key={index}
                    addedItems={addedItems}
                    setAddedItems={setAddedItems}
                    index={index}
                />
            ))}

            <div className="row">
                <div className="col-8 d-grid">
                    <Button className="btn-teal-dark" onClick={submit.bind(this, "check")}>
                        {isCheckLoading && (
                            <Spinner className="me-2" animation="grow" size="sm" />
                        )}
                        Check
                    </Button>
                </div>
                <div className="col-4 d-grid">
                    <Button variant="warning" onClick={submit.bind(this, "queue")}>
                        {isQueueLoading && (
                            <Spinner className="me-2" animation="grow" size="sm" />
                        )}
                        Queue
                    </Button>
                </div>
            </div>
        </>
    );
}

CheckBuy.propTypes = {
    addedItems: PropTypes.array.isRequired,
    setAddedItems: PropTypes.func.isRequired,
    buy: PropTypes.object.isRequired,
};

export default CheckBuy;
