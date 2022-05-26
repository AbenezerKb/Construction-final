import React from "react";
import PropTypes from "prop-types";
import IssueDecliendMaterial from "./fragments/IssueDeclinedMaterial";
import { Button } from "react-bootstrap";
import { useNavigate } from "react-router-dom";

function DeclinedIssue({ issue, addedItems, FormRow }) {
    const navigate = useNavigate();

    function goBack() {
        navigate(-1);
    }

    return (
        <>
            <FormRow
                labelL="Declined By"
                valueL={
                    issue.approvedBy
                        ? `${issue.approvedBy.fName} ${issue.approvedBy.mName}`
                        : ""
                }
                labelR="Declined On"
                valueR={
                    issue.approveDate
                        ? new Date(issue.approveDate).toLocaleString()
                        : ""
                }
            />

            <h1 className="display-6 text-center mt-2 mb-4">
                Issue Request Has Been Declined
            </h1>

            {addedItems.map((item, index) => (
                <IssueDecliendMaterial
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

DeclinedIssue.propTypes = {
    issue: PropTypes.object.isRequired,
    addedItems: PropTypes.array.isRequired,
};

export default DeclinedIssue;
