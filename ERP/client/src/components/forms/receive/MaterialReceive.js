import React from "react";
import PropTypes from "prop-types";
import ReceiveRequest from "./fragments/ReceiveRequest";
import { Button, Form, Spinner } from "react-bootstrap";
import { requestReceive } from "../../../api/receive";
import { useMutation, useQueryClient } from "react-query";
import ConnectionError from "../../fragments/ConnectionError";

function RequestReceive({ addedItems, setAddedItems, receive }) {
    const queryClient = useQueryClient();

    const {
        isLoading: isRequestLoading,
        isError: isRequestError,
        mutate: submitRequestReceive,
    } = useMutation(requestReceive, {
        onSuccess: (res) =>
            queryClient.invalidateQueries(["receive", receive.receiveId.toString()]),
    });


    function submit(e) {
        e.preventDefault();
        if (isRequestLoading) return;

        const data = {
            receiveId: receive.receiveId,
            deliveredById: receive.deliveredById,
            receiveItems: [],
        };

        for (let item of addedItems) {
            let tempItem = {
                itemId: Number(item.itemId),
                qtyReceived: Number(item.qtyReceived),
                receiveRemark: item.receiveRemark,
            };

            data.receiveItems.push(tempItem);
        } 

        submitRequestReceive(data);
    }

    if (isRequestError) return <ConnectionError />;

    return (
        <>
            <Form onSubmit={submit}>

                {addedItems.map((item, index) => (
                    <ReceiveRequest
                        key={index}
                        addedItems={addedItems}
                        setAddedItems={setAddedItems}
                        index={index}
                    />
                ))}

            <div className="row">
                <div className="d-grid">
                    <Button className="btn-teal-dark" type="submit">
                        {isRequestLoading && (
                            <Spinner className="me-2" animation="grow" size="sm" />
                        )}
                        Receive
                    </Button>
                </div>
            </div>
                
            </Form>
        </>
    );
}

RequestReceive.propTypes = {
    addedItems: PropTypes.array.isRequired,
    setAddedItems: PropTypes.func.isRequired,
    receive: PropTypes.object.isRequired,
};

export default RequestReceive;
