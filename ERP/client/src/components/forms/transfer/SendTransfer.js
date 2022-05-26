import React from "react";
import PropTypes from "prop-types";
import TransferSendMaterial from "./fragments/TransferSendMaterial";
import TransferSendEquipment from "./fragments/TransferSendEquipment";
import { Button, Form, Spinner } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import { sendTransfer } from "../../../api/transfer";
import { useMutation, useQueryClient } from "react-query";
import ConnectionError from "../../fragments/ConnectionError";
import { ITEMTYPE } from "../../../Constants";

function SendTransfer({ addedItems, setAddedItems, transfer }) {
    const queryClient = useQueryClient();

    const {
        isLoading: isSendLoading,
        isError: isSendError,
        error: sendError,
        mutate: submitSendTransfer,
    } = useMutation(sendTransfer, {
        onSuccess: (res) =>
            queryClient.invalidateQueries(["transfer", transfer.transferId.toString()]),
    });

    function submit(e) {
        e.preventDefault();
        if (isSendLoading) return;

        const data = {
            transferId: transfer.transferId,
            transferItems: [],
        };

        for (let item of addedItems) {
            let tempItem = {
                itemId: Number(item.itemId),
                sendRemark: item.sendRemark,
            };

            if (item.type === ITEMTYPE.EQUIPMENT) {
                tempItem.equipmentModelId = item.equipmentModelId;
                tempItem["equipmentAssetIds"] = item.equipmentAssets.map((ea) => ea.value);
            }

            data.transferItems.push(tempItem);
        }

        submitSendTransfer(data);
    }

    if (isSendError) return <ConnectionError status={sendError?.response?.status} />;

    return (
        <>
            <Form onSubmit={submit}>
                {addedItems.map((item, index) =>
                    item.type === ITEMTYPE.MATERIAL ? (
                        <TransferSendMaterial
                            key={index}
                            addedItems={addedItems}
                            setAddedItems={setAddedItems}
                            index={index}
                        />
                    ) : (
                        <TransferSendEquipment
                            key={index}
                            addedItems={addedItems}
                            setAddedItems={setAddedItems}
                            index={index}
                        />
                    )
                )}

                <div className="d-grid">
                    <Button className="btn-teal-dark" type="submit">
                        {isSendLoading && <Spinner className="me-2" animation="grow" size="sm" />}
                        Transfer Out
                    </Button>
                </div>
            </Form>
        </>
    );
}

SendTransfer.propTypes = {
    addedItems: PropTypes.array.isRequired,
    setAddedItems: PropTypes.func.isRequired,
    transfer: PropTypes.object.isRequired,
};

export default SendTransfer;
