import React from "react";
import PropTypes from "prop-types";
import BuyApproveMaterial from "./fragments/BuyApproveMaterial";
import { Button, Spinner } from "react-bootstrap";
import { approveBuy, declineBuy } from "../../../api/buy";
import { useMutation, useQueryClient } from "react-query";
import ConnectionError from "../../fragments/ConnectionError";

function ApproveBuy({ addedItems, setAddedItems, buy }) {
    const queryClient = useQueryClient();

    const {
        isLoading: isApproveLoading,
        isError: isApproveError,
        mutate: submitApproveBuy,
    } = useMutation(approveBuy, {
        onSuccess: (res) =>
            queryClient.invalidateQueries(["buy", buy.buyId.toString()]),
    });

    const {
        isLoading: isDeclineLoading,
        isError: isDeclineError,
        mutate: submitDeclineBuy,
    } = useMutation(declineBuy, {
        onSuccess: (res) =>
            queryClient.invalidateQueries(["buy", buy.buyId.toString()]),
    });

    function submit(type, e) {
        e.preventDefault();
        if (isApproveLoading || isDeclineLoading) return;

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

        if (type === "decline") submitDeclineBuy(data);
        else submitApproveBuy(data);
    }

    if (isApproveError || isDeclineError) return <ConnectionError />;

    return (
        <>
            {addedItems.map((item, index) => (
                <BuyApproveMaterial
                    key={index}
                    addedItems={addedItems}
                    setAddedItems={setAddedItems}
                    index={index}
                />
            ))}

            <div className="row">
                <div className="col-8 d-grid">
                    <Button className="btn-teal-dark" onClick={submit.bind(this, "approve")}>
                        {isApproveLoading && (
                            <Spinner className="me-2" animation="grow" size="sm" />
                        )}
                        Approve
                    </Button>
                </div>
                <div className="col-4 d-grid">
                    <Button variant="danger" onClick={submit.bind(this, "decline")}>
                        {isDeclineLoading && (
                            <Spinner className="me-2" animation="grow" size="sm" />
                        )}
                        Decline
                    </Button>
                </div>
            </div>
        </>
    );
}

ApproveBuy.propTypes = {
    addedItems: PropTypes.array.isRequired,
    setAddedItems: PropTypes.func.isRequired,
    buy: PropTypes.object.isRequired,
};

export default ApproveBuy;
