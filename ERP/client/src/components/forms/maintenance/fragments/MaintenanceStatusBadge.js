import { Badge } from "react-bootstrap";
import { MAINTENANCESTATUS } from "../../../../Constants";

function MaintenanceStatusBadge(status) {
    const color = ["danger", "info", "success", "secondary"];

    return (
        <Badge pill className="mt-1" bg={color[status]} style={{ width: "90px", fontSize: "14px" }}>
            {MAINTENANCESTATUS[status]}
        </Badge>
    );
}

export default MaintenanceStatusBadge;
