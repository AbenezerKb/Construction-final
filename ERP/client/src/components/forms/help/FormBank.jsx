import { Container, Form } from "react-bootstrap";
import Header from "../../layouts/Header";
import { url as host } from "../../../api/api";
import { FaFolderOpen, FaDownload } from "react-icons/fa";

function FormBank() {
    const fileName = "blank_forms.pdf";
    return (
        <div>
            <Header title="Form Bank" />
            <Container>
                <div className="display-6 pt-4 mt-5 fs-3 text-center">
                    This file has blank forms that can be used as a backup if the system can't be
                    used for any reason.
                </div>
                <div className="display-6 mt-2 mb-5 fs-5 text-center">
                    Download and print the files to use them.
                </div>
                <div className="row d-flex flex-row justify-content-between py-3 mx-5 shadow rounded">
                    <div className="col">
                        <Form.Label className="mt-2 ms-3">Blank Forms</Form.Label>
                    </div>
                    <div className="col d-grid">
                        <a
                            className="btn btn-teal"
                            href={`${host}file/${fileName}`}
                            target="_blank"
                        >
                            <FaFolderOpen className="me-2 mb-1" />
                            Open
                        </a>
                    </div>
                    <div className="col d-grid">
                        <a className="btn btn-teal mx-2" href={`${host}file/download/${fileName}`}>
                            <FaDownload className="me-2 mb-1" />
                            Download
                        </a>
                    </div>
                </div>
            </Container>
        </div>
    );
}

export default FormBank;
