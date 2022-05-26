import { Badge } from "react-bootstrap";
import { BULKPURCHASESTATUS } from "../../../../Constants";

function BulkPurchaseStatusBadge(status) {
    const color = ["danger", "warning", "secondary", "success", "info"];

    return (
        <Badge pill className="mt-1" bg={color[status]} style={{ width: "90px", fontSize: "14px" }}>
            {BULKPURCHASESTATUS[status]}
        </Badge>
    );
}

export default BulkPurchaseStatusBadge;
