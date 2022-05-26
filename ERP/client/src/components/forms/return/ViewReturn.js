import PropTypes from "prop-types";
import { Button } from "react-bootstrap";
import ReturnViewEquipment from "./fragments/ReturnViewEquipment";
import { useNavigate } from "react-router-dom";

function ViewReturn({ addedItems }) {
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
                        <th>Borrowed By</th>
                        <th>Handed Over On</th>
                        <th>Asset Number</th>
                        <th>Serial Number</th>
                        <th>Damage</th>
                        <th>Return Remark</th>
                    </tr>
                </thead>
                <tbody className="d-block d-print-table-row-group">
                    {addedItems.map((item, index) => (
                        <ReturnViewEquipment key={index} addedItems={addedItems} index={index} />
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

ViewReturn.propTypes = {
    addedItems: PropTypes.array.isRequired,
};

export default ViewReturn;
