/*
 * Yagna Market API
 *
 *  ## Yagna Market The Yagna Market is a core component of the Yagna Network, which enables computational Offers and Demands circulation. The Market is open for all entities willing to buy computations (Demands) or monetize computational resources (Offers). ## Yagna Market API The Yagna Market API is the entry to the Yagna Market through which Requestors and Providers can publish their Demands and Offers respectively, find matching counterparty, conduct negotiations and make an agreement.  This version of Market API conforms with capability level 1 of the <a href=\"https://docs.google.com/document/d/1Zny_vfgWV-hcsKS7P-Kdr3Fb0dwfl-6T_cYKVQ9mkNg\"> Market API specification</a>.  Market API contains two roles: Requestors and Providers which are symmetrical most of the time (excluding agreement phase). 
 *
 * OpenAPI spec version: 1.4.2
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace GolemMarketMockAPI.MarketAPI.Models
{ 
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class Proposal : DemandOfferBase, IEquatable<Proposal>
    { 
        /// <summary>
        /// Gets or Sets ProposalId
        /// </summary>
        [DataMember(Name="proposalId")]
        public string ProposalId { get; set; }

        /// <summary>
        /// Gets or Sets IssuerId
        /// </summary>
        [DataMember(Name="issuerId")]
        public string IssuerId { get; set; }

        /// <summary>
        /// * `Initial` - proposal arrived from the market as response to subscription * `Draft` - bespoke counter-proposal issued by one party directly to other party (negotiation phase) * `Rejected` by other party * `Accepted` - promoted into the Agreement draft * `Expired` - not accepted nor rejected before validity period 
        /// </summary>
        /// <value>* `Initial` - proposal arrived from the market as response to subscription * `Draft` - bespoke counter-proposal issued by one party directly to other party (negotiation phase) * `Rejected` by other party * `Accepted` - promoted into the Agreement draft * `Expired` - not accepted nor rejected before validity period </value>
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public enum StateEnum
        {
            /// <summary>
            /// Enum InitialEnum for Initial
            /// </summary>
            [EnumMember(Value = "Initial")]
            InitialEnum = 0,
            /// <summary>
            /// Enum DraftEnum for Draft
            /// </summary>
            [EnumMember(Value = "Draft")]
            DraftEnum = 1,
            /// <summary>
            /// Enum RejectedEnum for Rejected
            /// </summary>
            [EnumMember(Value = "Rejected")]
            RejectedEnum = 2,
            /// <summary>
            /// Enum AcceptedEnum for Accepted
            /// </summary>
            [EnumMember(Value = "Accepted")]
            AcceptedEnum = 3,
            /// <summary>
            /// Enum ExpiredEnum for Expired
            /// </summary>
            [EnumMember(Value = "Expired")]
            ExpiredEnum = 4        }

        /// <summary>
        /// * &#x60;Initial&#x60; - proposal arrived from the market as response to subscription * &#x60;Draft&#x60; - bespoke counter-proposal issued by one party directly to other party (negotiation phase) * &#x60;Rejected&#x60; by other party * &#x60;Accepted&#x60; - promoted into the Agreement draft * &#x60;Expired&#x60; - not accepted nor rejected before validity period 
        /// </summary>
        /// <value>* &#x60;Initial&#x60; - proposal arrived from the market as response to subscription * &#x60;Draft&#x60; - bespoke counter-proposal issued by one party directly to other party (negotiation phase) * &#x60;Rejected&#x60; by other party * &#x60;Accepted&#x60; - promoted into the Agreement draft * &#x60;Expired&#x60; - not accepted nor rejected before validity period </value>
        [DataMember(Name="state")]
        public StateEnum? State { get; set; }

        /// <summary>
        /// id of the proposal from other side which this proposal responds to 
        /// </summary>
        /// <value>id of the proposal from other side which this proposal responds to </value>
        [DataMember(Name="prevProposalId")]
        public string PrevProposalId { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Proposal {\n");
            sb.Append("  ProposalId: ").Append(ProposalId).Append("\n");
            sb.Append("  IssuerId: ").Append(IssuerId).Append("\n");
            sb.Append("  State: ").Append(State).Append("\n");
            sb.Append("  PrevProposalId: ").Append(PrevProposalId).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public  new string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Proposal)obj);
        }

        /// <summary>
        /// Returns true if Proposal instances are equal
        /// </summary>
        /// <param name="other">Instance of Proposal to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Proposal other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    ProposalId == other.ProposalId ||
                    ProposalId != null &&
                    ProposalId.Equals(other.ProposalId)
                ) && 
                (
                    IssuerId == other.IssuerId ||
                    IssuerId != null &&
                    IssuerId.Equals(other.IssuerId)
                ) && 
                (
                    State == other.State ||
                    State != null &&
                    State.Equals(other.State)
                ) && 
                (
                    PrevProposalId == other.PrevProposalId ||
                    PrevProposalId != null &&
                    PrevProposalId.Equals(other.PrevProposalId)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                    if (ProposalId != null)
                    hashCode = hashCode * 59 + ProposalId.GetHashCode();
                    if (IssuerId != null)
                    hashCode = hashCode * 59 + IssuerId.GetHashCode();
                    if (State != null)
                    hashCode = hashCode * 59 + State.GetHashCode();
                    if (PrevProposalId != null)
                    hashCode = hashCode * 59 + PrevProposalId.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(Proposal left, Proposal right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Proposal left, Proposal right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
