import React from "react";
import { Card, Button } from "react-bootstrap";
import { Link } from "react-router-dom";
import formatRelative from "date-fns/formatDistance";
import PropTypes from "prop-types";
import { FaMinus } from "react-icons/fa";
import { useMutation, useQueryClient } from "react-query";
import { clearNotification } from "../../api/notification";

function SingleNotification({ title, notificationId, content, target, date, now }) {
    const queryClient = useQueryClient();

    const { mutate } = useMutation(clearNotification, {
        onSuccess: () => queryClient.invalidateQueries(["notifications"]),
    });

    function handleClear(e) {
        mutate(notificationId);
    }

    return (
        <Card className="mb-2 pb-2 border-0 border-bottom">
            <span>
                <strong className="text-teal">{title}</strong>
                <Button
                    variant="transparent"
                    className="float-end py-0 px-1"
                    onClick={handleClear}
                    style={{ position: "relative", zIndex: "99" }}
                >
                    <FaMinus className="fs-5 btn-notification-clear mb-1" />{" "}
                </Button>
            </span>
            {content}
            <div className="text-secondary fw-light">
                {formatRelative(new Date(date), new Date(now))} ago
            </div>

            <Link className="stretched-link" to={target} />
        </Card>
    );
}

SingleNotification.propTypes = {
    title: PropTypes.string.isRequired,
    notificationId: PropTypes.number.isRequired,
    content: PropTypes.string.isRequired,
    target: PropTypes.string.isRequired,
    date: PropTypes.string.isRequired,
    now: PropTypes.string.isRequired,
};

export default SingleNotification;
