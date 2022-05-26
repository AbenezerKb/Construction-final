import { Badge } from "react-bootstrap";
import { ISSUESTATUS } from "../../../../Constants";

function IssueStatusBadge(status) {
    const color = ["danger", "info", "success", "secondary"];

    return (
        <Badge pill className="mt-1" bg={color[status]} style={{ width: "90px", fontSize: "14px" }}>
            {ISSUESTATUS[status]}
        </Badge>
    );
}

export default IssueStatusBadge;
