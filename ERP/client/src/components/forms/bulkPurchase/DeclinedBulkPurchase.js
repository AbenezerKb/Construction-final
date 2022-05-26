import React from "react"
import PropTypes from "prop-types"
import BulkPurchaseDeclined from "./fragments/BulkPurchaseDeclined"
import { Button } from "react-bootstrap"
import { useNavigate } from "react-router-dom"

function DeclinedBulkPurchase({ bulkPurchase, addedItems, FormRow }) {
  const navigate = useNavigate()
  const goBack = () => navigate(-1)
  return (
    <>
      <FormRow
        labelL="Declined By"
        valueL={
          bulkPurchase.approvedBy
            ? `${bulkPurchase.approvedBy.fName} ${bulkPurchase.approvedBy.mName}`
            : ""
        }
        labelR="Declined On"
        valueR={
          bulkPurchase.approveDate
            ? new Date(bulkPurchase.approveDate).toLocaleString()
            : ""
        }
      />

      <h1 className="display-6 text-center mt-2 mb-4">
        BulkPurchase Request Has Been Declined
      </h1>

      {addedItems.map((item, index) => (
        <BulkPurchaseDeclined
          key={index}
          addedItems={addedItems}
          index={index}
        />
      ))}

      <div className="d-grid">
        <Button className="btn-teal" onClick={goBack}>
          Back
        </Button>
      </div>
    </>
  )
}

DeclinedBulkPurchase.propTypes = {
  bulkPurchase: PropTypes.object.isRequired,
  addedItems: PropTypes.array.isRequired,
}

export default DeclinedBulkPurchase
