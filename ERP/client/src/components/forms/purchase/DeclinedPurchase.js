import React from "react"
import PropTypes from "prop-types"
import PurchaseDeclinedMaterial from "./fragments/PurchaseDeclinedMaterial"
import PurchaseDeclinedEquipment from "./fragments/PurchaseDeclinedEquipment"
import { ITEMTYPE } from "../../../Constants"
import { Button } from "react-bootstrap"
import { useNavigate } from "react-router-dom"

function DeclinedPurchase({ purchase, addedItems, FormRow }) {
  const navigate = useNavigate()
  const goBack = () => navigate(-1)
  return (
    <>
      <FormRow
        labelL="Declined By"
        valueL={
          purchase.approvedBy
            ? `${purchase.approvedBy.fName} ${purchase.approvedBy.mName}`
            : ""
        }
        labelR="Declined On"
        valueR={
          purchase.approveDate
            ? new Date(purchase.approveDate).toLocaleString()
            : ""
        }
      />

      <h1 className="display-6 text-center mt-2 mb-4">
        Purchase Request Has Been Declined
      </h1>

      {addedItems.map((item, index) =>
        item.type === ITEMTYPE.MATERIAL ? (
          <PurchaseDeclinedMaterial
            key={index}
            addedItems={addedItems}
            index={index}
          />
        ) : (
          <PurchaseDeclinedEquipment
            key={index}
            addedItems={addedItems}
            index={index}
          />
        )
      )}

      <div className="d-grid">
        <Button className="btn-teal" onClick={goBack}>
          Back
        </Button>
      </div>
    </>
  )
}

DeclinedPurchase.propTypes = {
  purchase: PropTypes.object.isRequired,
  addedItems: PropTypes.array.isRequired,
}

export default DeclinedPurchase
