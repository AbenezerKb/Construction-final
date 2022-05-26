import PropTypes from "prop-types";
import { Button } from "react-bootstrap";
import BorrowViewEquipment from "./fragments/BorrowViewEquipment";
import { useNavigate } from "react-router-dom";

function ViewBorrow({ addedItems }) {
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
                        <th>Model</th>
                        <th>Description</th>
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
                        <BorrowViewEquipment key={index} addedItems={addedItems} index={index} />
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

ViewBorrow.propTypes = {
    addedItems: PropTypes.array.isRequired,
};

export default ViewBorrow;
