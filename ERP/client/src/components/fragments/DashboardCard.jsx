import { useState } from "react";
import ReactCardFlip from "react-card-flip";
import { Card } from "react-bootstrap";
import { Link } from "react-router-dom";

function DashboardCard({ img, title, links }) {
    const [flipped, setFlipped] = useState(false);
    return (
        <ReactCardFlip isFlipped={flipped} flipDirection="vertical">
            <Card className="border-0 shadow" onClick={() => setFlipped((p) => !p)}>
                <Card.Img variant="top" src={img} />
                <h1 className="text-center display-6 fs-4 mb-4">{title}</h1>
            </Card>

            <Card className="border-0 shadow" onClick={() => setFlipped((p) => !p)}>
                <h1 className="text-center display-6 fs-4 mt-4 mb-4">{title}</h1>
                <div className="d-flex flex-column gap-1 mx-2 mb-2">
                    {links?.map((link, index) => (
                        <Link
                            to={link.to}
                            key={index}
                            className="btn btn-teal"
                            style={{ border: 0 }}
                        >
                            {link.label}
                        </Link>
                    ))}
                </div>
            </Card>
        </ReactCardFlip>
    );
}

export default DashboardCard;
