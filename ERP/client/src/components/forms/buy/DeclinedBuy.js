import React from "react";
import PropTypes from "prop-types";
import BuyDecliendMaterial from "./fragments/BuyDeclinedMaterial";

function DeclinedBuy({ buy, addedItems, FormRow }) {
    return (
        <>
            <FormRow
                labelL="Declined By"
                valueL={
                    buy.approvedBy
                        ? `${buy.approvedBy.fName} ${buy.approvedBy.mName}`
                        : ""
                }
                labelR="Declined On"
                valueR={
                    buy.approveDate 
                        ? new Date(buy.approveDate).toLocaleString() 
                        : ""}
            />

            <h1 className="display-6 text-center mt-2 mb-4">
                Buy Request Has Been Declined
            </h1>

            {addedItems.map((item, index) => (
                <BuyDecliendMaterial 
                    key={index} addedItems={addedItems} 
                    index={index} />
            ))}
        </>
    );
}

DeclinedBuy.propTypes = {
    buy: PropTypes.object.isRequired,
    addedItems: PropTypes.array.isRequired,
};

export default DeclinedBuy;
