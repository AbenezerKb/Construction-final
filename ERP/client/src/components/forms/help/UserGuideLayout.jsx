import { useState, createRef, useEffect } from "react";
import { Container, Row, Col, Navbar, Nav } from "react-bootstrap";
import useScrollSpy from "react-use-scrollspy";
import Test from "./Test";

function UserGuideLayout({ children }) {
    const [sectionRefs, setSectionRefs] = useState([]);

    let activeSection = useScrollSpy({
        sectionElementRefs: sectionRefs,
        offsetPx: -80,
    });

    useEffect(
        () =>
            setSectionRefs((elRefs) =>
                Array(children.length)
                    .fill()
                    .map((_, i) => elRefs[i] || createRef())
            ),
        []
    );

    return (
        <Container>
            <Row className="ps-3 g-5">
                <Col
                    style={{
                        paddingBottom: "10vh",
                    }}
                >
                    {children?.map((section, index) => (
                        <div key={index}>
                            <div ref={sectionRefs[index]}>
                                <h1 className="display-6 fs-2 pt-5">{section.props.title}</h1>
                                {section}
                            </div>
                        </div>
                    ))}
                </Col>
                <Col lg={3} className="d-print-none">
                    <div
                        className="position-fixed w-25"
                        style={{
                            height: "90vh",
                            overflow: "auto",
                            scrollBehavior: "smooth",
                        }}
                    >
                        <Navbar className=" flex-column align-items-stretch">
                            <Navbar.Brand>User's Guide</Navbar.Brand>

                            <Nav className="nav-pills flex-column">
                                {children?.map((section, index) => (
                                    <Nav.Link
                                        key={index}
                                        className={activeSection === index && "help-teal-dark"}
                                        onClick={() => sectionRefs[index].current.scrollIntoView()}
                                    >
                                        {section.props.title}
                                    </Nav.Link>
                                ))}
                            </Nav>
                        </Navbar>
                    </div>
                </Col>
            </Row>
        </Container>
    );
}

export default UserGuideLayout;
