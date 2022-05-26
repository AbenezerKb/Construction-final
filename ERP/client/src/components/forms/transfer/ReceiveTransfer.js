import React, { useRef } from "react";
import PropTypes from "prop-types";
import TransferReceiveMaterial from "./fragments/TransferReceiveMaterial";
import TransferReceiveEquipment from "./fragments/TransferReceiveEquipment";
import { Button, Form, Spinner } from "react-bootstrap";
import { receiveTransfer } from "../../../api/transfer";
import { useMutation, useQueryClient } from "react-query";
import ConnectionError from "../../fragments/ConnectionError";
import { ITEMTYPE } from "../../../Constants";

function ReceiveTransfer({ addedItems, setAddedItems, transfer }) {
    const queryClient = useQueryClient();
    const vehiclePlateRef = useRef();
    const driverRef = useRef();

    const {
        isLoading: isReceiveLoading,
        isError: isReceiveError,
        error: receiveError,
        mutate: submitReceiveTransfer,
    } = useMutation(receiveTransfer, {
        onSuccess: (res) =>
            queryClient.invalidateQueries(["transfer", transfer.transferId.toString()]),
    });

    function submit(e) {
        e.preventDefault();
        if (isReceiveLoading) return;

        const data = {
            transferId: transfer.transferId,
            deliveredBy: driverRef.current.value,
            vehiclePlateNo: vehiclePlateRef.current.value,
            transferItems: [],
        };

        for (let item of addedItems) {
            let tempItem = {
                itemId: Number(item.itemId),
                receiveRemark: item.receiveRemark,
            };

            if (item.type === ITEMTYPE.EQUIPMENT) tempItem.equipmentModelId = item.equipmentModelId;

            data.transferItems.push(tempItem);
        }

        submitReceiveTransfer(data);
    }

    function DeliveredByForm() {
        return (
            <div className="row mx-2">
                <div className="col">
                    <Form.Group className="mb-3">
                        <div className="row">
                            <div className="col-3 mt-1">
                                <Form.Label>Delivered By</Form.Label>
                            </div>
                            <div className="col">
                                <Form.Control type="text" required ref={driverRef} />
                            </div>
                        </div>
                    </Form.Group>
                </div>
                <div className="col">
                    <Form.Group className="mb-3">
                        <div className="row">
                            <div className="col-3 mt-1">
                                <Form.Label>Vehicle Plate No.</Form.Label>
                            </div>
                            <div className="col">
                                <Form.Control type="text" required ref={vehiclePlateRef} />
                            </div>
                        </div>
                    </Form.Group>
                </div>
            </div>
        );
    }

    if (isReceiveError) return <ConnectionError status={receiveError?.response?.status} />;

    return (
        <>
            <Form onSubmit={submit}>
                {addedItems.map((item, index) =>
                    item.type == ITEMTYPE.MATERIAL ? (
                        <TransferReceiveMaterial
                            key={index}
                            addedItems={addedItems}
                            setAddedItems={setAddedItems}
                            index={index}
                        />
                    ) : (
                        <TransferReceiveEquipment
                            key={index}
                            addedItems={addedItems}
                            setAddedItems={setAddedItems}
                            index={index}
                        />
                    )
                )}

                <DeliveredByForm />

                <div className="d-grid">
                    <Button className="btn-teal-dark" type="submit">
                        {isReceiveLoading && (
                            <Spinner className="me-2" animation="grow" size="sm" />
                        )}
                        Transfer In
                    </Button>
                </div>
            </Form>
        </>
    );
}

ReceiveTransfer.propTypes = {
    addedItems: PropTypes.array.isRequired,
    setAddedItems: PropTypes.func.isRequired,
    transfer: PropTypes.object.isRequired,
};

export default ReceiveTransfer;
