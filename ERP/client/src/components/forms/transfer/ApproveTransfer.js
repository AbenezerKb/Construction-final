import React from "react";
import PropTypes from "prop-types";
import TransferApproveMaterial from "./fragments/TransferApproveMaterial";
import TransferApproveEquipment from "./fragments/TransferApproveEquipment";
import { Button, Spinner, Form } from "react-bootstrap";
import { approveTransfer, declineTransfer } from "../../../api/transfer";
import { useMutation, useQueryClient } from "react-query";
import ConnectionError from "../../fragments/ConnectionError";
import { ITEMTYPE } from "../../../Constants";

function ApproveTransfer({ addedItems, setAddedItems, transfer }) {
    const queryClient = useQueryClient();

    const {
        isLoading: isApproveLoading,
        isError: isApproveError,
        error: approveError,
        mutate: submitApproveTransfer,
    } = useMutation(approveTransfer, {
        onSuccess: (res) =>
            queryClient.invalidateQueries(["transfer", transfer.transferId.toString()]),
    });

    const {
        isLoading: isDeclineLoading,
        isError: isDeclineError,
        error: declineError,
        mutate: submitDeclineTransfer,
    } = useMutation(declineTransfer, {
        onSuccess: (res) =>
            queryClient.invalidateQueries(["transfer", transfer.transferId.toString()]),
    });

    function submit(e) {
        e.preventDefault();
        if (isApproveLoading || isDeclineLoading) return;
        const type = e.nativeEvent.submitter.name;

        const data = {
            transferId: transfer.transferId,
            transferItems: [],
        };

        for (let item of addedItems) {
            let tempItem = {
                itemId: Number(item.itemId),
                qtyApproved: Number(item.qtyApproved),
                approveRemark: item.approveRemark,
            };

            if (item.type === ITEMTYPE.EQUIPMENT) tempItem.equipmentModelId = item.equipmentModelId;

            data.transferItems.push(tempItem);
        }

        if (type === "decline") submitDeclineTransfer(data);
        else submitApproveTransfer(data);
    }

    if (isApproveError || isDeclineError)
        return (
            <ConnectionError
                status={approveError?.response?.status && declineError?.response?.status}
            />
        );

    return (
        <Form onSubmit={submit}>
            {addedItems.map((item, index) =>
                item.type == ITEMTYPE.MATERIAL ? (
                    <TransferApproveMaterial
                        key={index}
                        addedItems={addedItems}
                        setAddedItems={setAddedItems}
                        index={index}
                    />
                ) : (
                    <TransferApproveEquipment
                        key={index}
                        addedItems={addedItems}
                        setAddedItems={setAddedItems}
                        index={index}
                    />
                )
            )}

            <div className="row">
                <div className="col-8 d-grid">
                    <Button className="btn-teal-dark" type="submit" name="approve">
                        {isApproveLoading && (
                            <Spinner className="me-2" animation="grow" size="sm" />
                        )}
                        Approve
                    </Button>
                </div>
                <div className="col-4 d-grid">
                    <Button variant="danger" type="submit" name="decline">
                        {isDeclineLoading && (
                            <Spinner className="me-2" animation="grow" size="sm" />
                        )}
                        Decline
                    </Button>
                </div>
            </div>
        </Form>
    );
}

ApproveTransfer.propTypes = {
    addedItems: PropTypes.array.isRequired,
    setAddedItems: PropTypes.func.isRequired,
    transfer: PropTypes.object.isRequired,
};

export default ApproveTransfer;
