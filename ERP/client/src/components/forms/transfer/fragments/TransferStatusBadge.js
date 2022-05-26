import { Badge } from "react-bootstrap";
import { TRANSFERSTATUS } from "../../../../Constants";

function TransferStatusBadge(status) {
    const color = ["danger", "info", "success", "primary", "secondary"];

    return (
        <Badge pill className="mt-1" bg={color[status]} style={{ width: "90px", fontSize: "14px" }}>
            {TRANSFERSTATUS[status]}
        </Badge>
    );
}

export default TransferStatusBadge;
