import PropTypes from "prop-types";
import { Row, Col, Card, Image } from "react-bootstrap";
import { fetchEquipmentCategories } from "../../../../api/category";
import { useQuery } from "react-query";
import { url as host } from "../../../../api/api";

function BorrowCategorySelection({ setCategoryId }) {
    const categoriesQuery = useQuery("equipmentcategories", fetchEquipmentCategories);

    function setCategory(id) {
        setCategoryId(id);
    }

    return (
        <Row xs={1} lg={2} className="g-4">
            {categoriesQuery.data?.map((category, index) => (
                <Col key={index}>
                    <Card
                        className="border-0 shadow"
                        style={{ cursor: "pointer" }}
                        onClick={() => setCategory(category.equipmentCategoryId)}
                    >
                        <Card.Img
                            variant="top"
                            src={`${host}file/${category.fileName}`}
                            style={{
                                width: "100%",
                                height: "12rem",
                                objectFit: "cover",
                            }}
                        />

                        <Card.ImgOverlay className="py-2 fs-4">
                            <div className="h-100 d-flex align-items-end">
                                <div
                                    className="text-white"
                                    style={{
                                        textShadow: "0 0 5px #000, 0 0 15px #000, 0 0 30px #000",
                                    }}
                                >
                                    {category.name}
                                </div>
                            </div>
                        </Card.ImgOverlay>
                    </Card>
                </Col>
            ))}
        </Row>
    );
}

BorrowCategorySelection.propTypes = {
    setCategoryId: PropTypes.func.isRequired,
};

export default BorrowCategorySelection;
