import { Badge } from "react-bootstrap";
import { BUYSTATUS } from "../../../../Constants";

function BuyStatusBadge(status) {
    const color = ["danger", "secondary", "warning", "primary", "success", "info"];

    return (
        <Badge pill className="mt-1" bg={color[status]} style={{ width: "90px", fontSize: "14px" }}>
            {BUYSTATUS[status]}
        </Badge>
    );
}

export default BuyStatusBadge;
