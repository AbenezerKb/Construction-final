import { Navbar, Container, Nav, Button } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import PropTypes from "prop-types";
import { FaArrowLeft, FaPrint } from "react-icons/fa";
import NotificationCanvas from "../notification/NotificationCanvas";

function Header({ title, showNotification, showPrint }) {
    const navigate = useNavigate();

    function goBack() {
        navigate(-1);
    }

    return (
        <Navbar className="navbar-dark bg-teal" expand="sm">
            <Container>
                <Button className="text-white" variant="transparent" onClick={goBack}>
                    <FaArrowLeft className="d-print-none" />
                </Button>
                <Navbar.Brand>{title}</Navbar.Brand>

                <Nav className="ms-auto">
                    {showPrint && (
                        <Button className="btn-teal-dark me-3" onClick={() => window.print()}>
                            <FaPrint className="me-2" />
                            Print
                        </Button>
                    )}
                    {showNotification && <NotificationCanvas />}
                </Nav>
            </Container>
        </Navbar>
    );
}

Header.propTypes = {
    title: PropTypes.string.isRequired,
    showNotification: PropTypes.bool,
    showPrint: PropTypes.bool,
};

Header.defaultProps = {
    showNotification: true,
    showPrint: false,
};

export default Header;
