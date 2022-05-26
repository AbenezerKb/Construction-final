import { Container, Table } from "react-bootstrap";
import { GENERALREPORTGROUPBY } from "../../../../Constants";
import PropTypes from "prop-types";

function SimpleReportTable({ title, data, groupBy }) {
    return (
        <>
            <h1 className="text-center mb-2 fs-4">{title}</h1>
            <Container fluid className="px-4 pb-4">
                <Table responsive striped bordered hover>
                    <thead>
                        <tr>
                            <th>#</th>
                            <th>
                                {
                                    GENERALREPORTGROUPBY[
                                        Object.keys(GENERALREPORTGROUPBY).find(
                                            (i) => GENERALREPORTGROUPBY[i].key === groupBy
                                        )
                                    ]?.label
                                }
                            </th>
                            <th>Quantity</th>
                            <th>Cost (Birr)</th>
                            <th>Current Value (Birr)</th>
                        </tr>
                    </thead>
                    <tbody>
                        {Object.values(data)?.map((current, index) => (
                            <tr key={index}>
                                <td>{index + 1}</td>
                                <td>{current.label}</td>
                                <td>{current.qty}</td>
                                <td>{current.cost}</td>
                                <td>{current.currentValue}</td>
                            </tr>
                        ))}
                    </tbody>
                </Table>
            </Container>
        </>
    );
}

SimpleReportTable.propTypes = {
    title: PropTypes.string.isRequired,
    data: PropTypes.object.isRequired,
    groupBy: PropTypes.string.isRequired,
};

export default SimpleReportTable;
