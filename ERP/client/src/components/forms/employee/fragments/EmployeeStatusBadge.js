import { Badge } from "react-bootstrap";
import { EMPLOYEESTATUS } from "../../../../Constants";

function EmployeeStatusBadge(status) {
    const color = ["info", "success"];

    return (
        <Badge pill className="mt-1" bg={color[status]} style={{ width: "90px", fontSize: "14px" }}>
            {EMPLOYEESTATUS[status]}
        </Badge>
    );
}

export default EmployeeStatusBadge;
