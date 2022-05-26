import { Badge } from "react-bootstrap";
import { BORROWSTATUS } from "../../../../Constants";

function BorrowStatusBadge(status) {
    const color = ["danger", "info", "success", "secondary"];

    return (
        <Badge pill className="mt-1" bg={color[status]} style={{ width: "90px", fontSize: "14px" }}>
            {BORROWSTATUS[status]}
        </Badge>
    );
}

export default BorrowStatusBadge;
