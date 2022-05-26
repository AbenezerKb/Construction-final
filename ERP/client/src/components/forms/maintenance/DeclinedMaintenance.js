import React from "react";
import PropTypes from "prop-types";
import BorrowDeclinedEquipment from "./fragments/MaintenanceDeclinedEquipment";
import { Button } from "react-bootstrap";
import { useNavigate } from "react-router-dom";

function DeclinedMaintenance({ maintenance, addedItems, FormRow }) {
    const navigate = useNavigate();

    function goBack() {
        navigate(-1);
    }

    return (
        <>
            <FormRow
                labelL="Declined By"
                valueL={
                    maintenance.approvedBy
                        ? `${maintenance.approvedBy.fName} ${maintenance.approvedBy.mName}`
                        : ""
                }
                labelR="Declined On"
                valueR={
                    maintenance.approveDate
                        ? new Date(maintenance.approveDate).toLocaleString()
                        : ""
                }
            />

            <h1 className="display-6 text-center mt-2 mb-4">
                Maintenance Request Has Been Declined
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

DeclinedMaintenance.propTypes = {
    maintenance: PropTypes.object.isRequired,
    addedItems: PropTypes.array.isRequired,
};

export default DeclinedMaintenance;
