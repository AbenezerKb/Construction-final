import PropTypes from "prop-types";
import { Button } from "react-bootstrap";
import IssueViewMaterial from "./fragments/IssueViewMaterial";
import { useNavigate } from "react-router-dom";

function ViewIssue({ addedItems }) {
    const navigate = useNavigate();

    function goBack() {
        navigate(-1);
    }

    return (
        <>
            <table className="d-block d-print-table table-bordered table-responsive">
                <thead>
                    <tr className="d-none d-print-table-row">
                        <th>#</th>
                        <th>Item</th>
                        <th>Specification</th>
                        <th>Qty Requested</th>
                        <th>Qty Approved</th>
                        <th>Unit Cost</th>
                        <th>Total Cost</th>
                        <th>Request Remark</th>
                        <th>Approve Remark</th>
                        <th>Hand Over Remark</th>
                    </tr>
                </thead>
                <tbody className="d-block d-print-table-row-group">
                    {addedItems.map((item, index) => (
                        <IssueViewMaterial key={index} addedItems={addedItems} index={index} />
                    ))}
                </tbody>
            </table>

            <div className="d-grid">
                <Button className="btn-teal" onClick={goBack}>
                    Back
                </Button>
            </div>
        </>
    );
}

ViewIssue.propTypes = {
    addedItems: PropTypes.array.isRequired,
};

export default ViewIssue;
