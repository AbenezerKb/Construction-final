import React from "react";
import PropTypes from "prop-types";
import BuyConfirmMaterial from "./fragments/BuyConfirmMaterial";
import { Button, Form, Spinner } from "react-bootstrap";
import { confirmBuy } from "../../../api/buy";
import { useMutation, useQueryClient } from "react-query";
import ConnectionError from "../../fragments/ConnectionError";

function ConfirmBuy({ addedItems, setAddedItems, buy }) {
    const queryClient = useQueryClient();

    const {
        isLoading: isConfirmLoading,
        isError: isConfirmError,
        mutate: submitConfirmBuy,
    } = useMutation(confirmBuy, {
        onSuccess: (res) =>
            queryClient.invalidateQueries(["buy", buy.buyId.toString()]),
    });


    function submit(e) {
        e.preventDefault();
        if (isConfirmLoading) return;

        const data = {
            buyId: buy.buyId,
            buyItems: [],
        };

        for (let item of addedItems) {
            let tempItem = {
                itemId: Number(item.itemId),
                qtyPurchased: Number(item.qtyPurchased),
                buyRemark: item.buyRemark,
            };

            data.buyItems.push(tempItem);
        } 

        submitConfirmPurchase(data);
    }

    if (isConfirmError) return <ConnectionError />;

    return (
        <>
            <Form onSubmit={submit}>

                {addedItems.map((item, index) => (
                    <PurchaseConfirmMaterial
                        key={index}
                        addedItems={addedItems}
                        setAddedItems={setAddedItems}
                        index={index}
                    />
                ))}

                <div className="d-grid">
                    <Button className="btn-teal-dark" type="submit">
                        {isConfirmLoading && (
                            <Spinner 
                                className="me-2" 
                                animation="grow" 
                                size="sm" />
                        )}
                        Confirm Purchase
                    </Button>
                </div>
            </Form>
        </>
    );
}

ConfirmPurchase.propTypes = {
    addedItems: PropTypes.array.isRequired,
    setAddedItems: PropTypes.func.isRequired,
    buy: PropTypes.object.isRequired,
};

export default ConfirmBuy;
