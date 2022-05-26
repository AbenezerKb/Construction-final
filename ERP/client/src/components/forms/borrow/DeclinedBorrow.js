import React from "react";
import PropTypes from "prop-types";
import BorrowDeclinedEquipment from "./fragments/BorrowDeclinedEquipment";
import { Button } from "react-bootstrap";
import { useNavigate } from "react-router-dom";

function DeclinedBorrow({ borrow, addedItems, FormRow }) {
    const navigate = useNavigate();

    function goBack() {
        navigate(-1);
    }

    return (
        <>
            <FormRow
                labelL="Declined By"
                valueL={
                    borrow.approvedBy
                        ? `${borrow.approvedBy.fName} ${borrow.approvedBy.mName}`
                        : ""
                }
                labelR="Declined On"
                valueR={
                    borrow.approveDate
                        ? new Date(borrow.approveDate).toLocaleString()
                        : ""
                }
            />

            <h1 className="display-6 text-center mt-2 mb-4">
                Borrow Request Has Been Declined
            </h1>

            {addedItems.map((item, index) => (
                <BorrowDeclinedEquipment
                    key={index}
                    addedItems={addedItems}
                    index={index}
                />
            ))}
            <div className="d-grid">
                <Button className="btn-teal" onClick={goBack}>
                    Back
                </Button>
            </div>
        </>
    );
}

DeclinedBorrow.propTypes = {
    borrow: PropTypes.object.isRequired,
    addedItems: PropTypes.array.isRequired,
};

export default DeclinedBorrow;
