import { Badge } from "react-bootstrap";
import { PURCHASESTATUS } from "../../../../Constants";

function PurchaseStatusBadge(status) {
    const color = ["danger", "warning", "secondary", "warning", "primary", "success", "info"];

    return (
        <Badge pill className="mt-1" bg={color[status]} style={{ width: "90px", fontSize: "14px" }}>
            {PURCHASESTATUS[status]}
        </Badge>
    );
}

export default PurchaseStatusBadge;
