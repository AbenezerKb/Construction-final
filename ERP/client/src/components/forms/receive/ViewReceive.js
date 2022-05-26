import React, { useRef } from "react"
import PropTypes from "prop-types"
import { Button } from "react-bootstrap"
import ReceiveViewItem from "./fragments/ReceiveViewItem"
import { useNavigate } from "react-router-dom"
import { ITEMTYPE } from "../../../Constants"

function ViewReceive({ addedItems }) {
  const navigate = useNavigate()

  function goBack() {
    navigate(-1)
  }

  return (
    <>
      <table className="d-block d-print-table table-bordered table-responsive">
        <thead>
          {addedItems[0].type === ITEMTYPE.EQUIPMENT ? (
            <tr className="d-none d-print-table-row">
              <th>#</th>
              <th>Item</th>
              <th>Model</th>
              <th>Description</th>
              <th>Qty Received</th>
              <th>Unit Cost</th>
              <th>Total Cost</th>
              <th>Receive Remark</th>
            </tr>
          ) : (
            <tr className="d-none d-print-table-row">
              <th>#</th>
              <th>Item</th>
              <th>Specification</th>
              <th>Qty Received</th>
              <th>Unit Cost</th>
              <th>Total Cost</th>
              <th>Receive Remark</th>
            </tr>
          )}
        </thead>
        <tbody className="d-block d-print-table-row-group">
          {addedItems.map((item, index) => (
            <ReceiveViewItem
              key={index}
              addedItems={addedItems}
              index={index}
            />
          ))}
        </tbody>
      </table>
      <div className="d-grid">
        <Button className="btn-teal" onClick={goBack}>
          Back
        </Button>
      </div>
    </>
  )
}

ViewReceive.propTypes = {
  addedItems: PropTypes.array.isRequired,
}

export default ViewReceive
