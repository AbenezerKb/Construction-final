import React from "react"
import PropTypes from "prop-types"
import BuyViewMaterial from "./fragments/BuyViewMaterial"
import { Button } from "react-bootstrap"
import { useNavigate } from "react-router-dom"

function ViewBuy({ addedItems }) {
  const navigate = useNavigate()

  function goBack() {
    navigate(-1)
  }

  return (
    <>
      <table className="d-block d-print-table table-bordered table-responsive">
        <thead>
          <tr className="d-none d-print-table-row">
            <th>#</th>
            <th>Item</th>
            <th>Specification</th>
            <th>Qty Requested</th>
            <th>Qty Approved</th>
            <th>Qty Bought</th>
            <th>Unit Cost</th>
            <th>Total Cost</th>
            <th>Request Remark</th>
            <th>Approve Remark</th>
            <th>Buy Remark</th>
          </tr>
        </thead>
        <tbody className="d-block d-print-table-row-group">
          {addedItems.map((item, index) => (
            <BuyViewMaterial
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

ViewBuy.propTypes = {
  addedItems: PropTypes.array.isRequired,
}

export default ViewBuy
