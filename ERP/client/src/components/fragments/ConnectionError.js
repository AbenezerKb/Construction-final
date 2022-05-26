import React from "react";
import PropTypes from "prop-types";
import {  Container } from "react-bootstrap";
import { Link } from "react-router-dom";

function ConnectionError({ status }) {
    function HomeButton() {
        return (
            <Container className="d-flex  pt-5 justify-content-center">
                <Link to="/" className="btn btn-teal">
                    Go to Home
                </Link>
            </Container>
        );
    }

    if (status === 401)
        return (
            <div className="pt-5">
                <h1 className="display-6 text-center mt-4 pt-4">
                    You Are Not Authorized To Access The Requested Data.
                </h1>
                <HomeButton />
            </div>
        );

    if (status === 403)
        return (
            <div className="pt-5">
                <h1 className="display-6 text-center mt-4 pt-4">
                    Sorry, You Don't Have Permission To Access The Requested Data.
                </h1>
                <h1 className="display-6 text-center pt-2 fs-4">
                    If This Is Not Expected To Happen Please Contact The Admin.
                </h1>
                <HomeButton />
            </div>
        );

    if (status === 404)
        return (
            <div className="pt-5">
                <h1 className="display-6 text-center mt-4 pt-4">
                    Sorry, We Cannot Seem To Find What You Requested.
                </h1>
                <HomeButton />
            </div>
        );
    return (
        <div className="pt-5">
            <h1 className="display-6 text-center mt-4 pt-4">Couldn't Connect, Please Try Again.</h1>
            <HomeButton />
        </div>
    );
}

ConnectionError.propTypes = {
    status: PropTypes.number.isRequired,
};

export default ConnectionError;
