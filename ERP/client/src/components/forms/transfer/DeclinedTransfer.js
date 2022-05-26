import React from "react";
import PropTypes from "prop-types";
import TransferDecliendMaterial from "./fragments/TransferDeclinedMaterial";
import TransferDecliendEquipment from "./fragments/TransferDeclinedEquipment";
import { ITEMTYPE } from "../../../Constants";
import { Button } from "react-bootstrap";
import { useNavigate } from "react-router-dom";

function DeclinedTransfer({ transfer, addedItems, FormRow }) {
    const navigate = useNavigate();
    const goBack = () => navigate(-1);
    return (
        <>
            <FormRow
                labelL="Declined By"
                valueL={
                    transfer.approvedBy
                        ? `${transfer.approvedBy.fName} ${transfer.approvedBy.mName}`
                        : ""
                }
                labelR="Declined On"
                valueR={transfer.approveDate ? new Date(transfer.approveDate).toLocaleString() : ""}
            />

            <h1 className="display-6 text-center mt-2 mb-4">Transfer Request Has Been Declined</h1>

            {addedItems.map((item, index) =>
                item.type === ITEMTYPE.MATERIAL ? (
                    <TransferDecliendMaterial key={index} addedItems={addedItems} index={index} />
                ) : (
                    <TransferDecliendEquipment key={index} addedItems={addedItems} index={index} />
                )
            )}
            <div className="d-grid">
                <Button className="btn-teal" onClick={goBack}>
                    Back
                </Button>
            </div>
        </>
    );
}

DeclinedTransfer.propTypes = {
    transfer: PropTypes.object.isRequired,
    addedItems: PropTypes.array.isRequired,
};

export default DeclinedTransfer;
