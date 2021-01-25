/* 
 * OpenBots Server API
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: v1
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using SwaggerDateConverter = OpenBots.Service.API.Client.SwaggerDateConverter;

namespace OpenBots.Service.API.Model
{
    /// <summary>
    /// ScheduleViewModel
    /// </summary>
    [DataContract]
        public partial class ScheduleViewModel :  IEquatable<ScheduleViewModel>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleViewModel" /> class.
        /// </summary>
        /// <param name="createdOn">createdOn.</param>
        /// <param name="id">id.</param>
        /// <param name="name">name.</param>
        /// <param name="agentId">agentId.</param>
        /// <param name="agentName">agentName.</param>
        /// <param name="cronExpression">cronExpression.</param>
        /// <param name="lastExecution">lastExecution.</param>
        /// <param name="nextExecution">nextExecution.</param>
        /// <param name="isDisabled">isDisabled.</param>
        /// <param name="projectId">projectId.</param>
        /// <param name="processId">processId.</param>
        /// <param name="processName">processName.</param>
        /// <param name="triggerName">triggerName.</param>
        /// <param name="startingType">startingType.</param>
        /// <param name="status">status.</param>
        /// <param name="expiryDate">expiryDate.</param>
        /// <param name="startDate">startDate.</param>
        /// <param name="createdBy">createdBy.</param>
        /// <param name="scheduleNow">scheduleNow.</param>
        public ScheduleViewModel(DateTime? createdOn = default(DateTime?), Guid? id = default(Guid?), string name = default(string), Guid? agentId = default(Guid?), string agentName = default(string), string cronExpression = default(string), DateTime? lastExecution = default(DateTime?), DateTime? nextExecution = default(DateTime?), bool? isDisabled = default(bool?), Guid? projectId = default(Guid?), Guid? processId = default(Guid?), string processName = default(string), string triggerName = default(string), string startingType = default(string), string status = default(string), DateTime? expiryDate = default(DateTime?), DateTime? startDate = default(DateTime?), string createdBy = default(string), bool? scheduleNow = default(bool?))
        {
            this.CreatedOn = createdOn;
            this.Id = id;
            this.Name = name;
            this.AgentId = agentId;
            this.AgentName = agentName;
            this.CronExpression = cronExpression;
            this.LastExecution = lastExecution;
            this.NextExecution = nextExecution;
            this.IsDisabled = isDisabled;
            this.ProjectId = projectId;
            this.ProcessId = processId;
            this.ProcessName = processName;
            this.TriggerName = triggerName;
            this.StartingType = startingType;
            this.Status = status;
            this.ExpiryDate = expiryDate;
            this.StartDate = startDate;
            this.CreatedBy = createdBy;
            this.ScheduleNow = scheduleNow;
        }
        
        /// <summary>
        /// Gets or Sets CreatedOn
        /// </summary>
        [DataMember(Name="createdOn", EmitDefaultValue=false)]
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id", EmitDefaultValue=false)]
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets AgentId
        /// </summary>
        [DataMember(Name="agentId", EmitDefaultValue=false)]
        public Guid? AgentId { get; set; }

        /// <summary>
        /// Gets or Sets AgentName
        /// </summary>
        [DataMember(Name="agentName", EmitDefaultValue=false)]
        public string AgentName { get; set; }

        /// <summary>
        /// Gets or Sets CronExpression
        /// </summary>
        [DataMember(Name="cronExpression", EmitDefaultValue=false)]
        public string CronExpression { get; set; }

        /// <summary>
        /// Gets or Sets LastExecution
        /// </summary>
        [DataMember(Name="lastExecution", EmitDefaultValue=false)]
        public DateTime? LastExecution { get; set; }

        /// <summary>
        /// Gets or Sets NextExecution
        /// </summary>
        [DataMember(Name="nextExecution", EmitDefaultValue=false)]
        public DateTime? NextExecution { get; set; }

        /// <summary>
        /// Gets or Sets IsDisabled
        /// </summary>
        [DataMember(Name="isDisabled", EmitDefaultValue=false)]
        public bool? IsDisabled { get; set; }

        /// <summary>
        /// Gets or Sets ProjectId
        /// </summary>
        [DataMember(Name="projectId", EmitDefaultValue=false)]
        public Guid? ProjectId { get; set; }

        /// <summary>
        /// Gets or Sets ProcessId
        /// </summary>
        [DataMember(Name="processId", EmitDefaultValue=false)]
        public Guid? ProcessId { get; set; }

        /// <summary>
        /// Gets or Sets ProcessName
        /// </summary>
        [DataMember(Name="processName", EmitDefaultValue=false)]
        public string ProcessName { get; set; }

        /// <summary>
        /// Gets or Sets TriggerName
        /// </summary>
        [DataMember(Name="triggerName", EmitDefaultValue=false)]
        public string TriggerName { get; set; }

        /// <summary>
        /// Gets or Sets StartingType
        /// </summary>
        [DataMember(Name="startingType", EmitDefaultValue=false)]
        public string StartingType { get; set; }

        /// <summary>
        /// Gets or Sets Status
        /// </summary>
        [DataMember(Name="status", EmitDefaultValue=false)]
        public string Status { get; set; }

        /// <summary>
        /// Gets or Sets ExpiryDate
        /// </summary>
        [DataMember(Name="expiryDate", EmitDefaultValue=false)]
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// Gets or Sets StartDate
        /// </summary>
        [DataMember(Name="startDate", EmitDefaultValue=false)]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or Sets CreatedBy
        /// </summary>
        [DataMember(Name="createdBy", EmitDefaultValue=false)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or Sets ScheduleNow
        /// </summary>
        [DataMember(Name="scheduleNow", EmitDefaultValue=false)]
        public bool? ScheduleNow { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ScheduleViewModel {\n");
            sb.Append("  CreatedOn: ").Append(CreatedOn).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  AgentId: ").Append(AgentId).Append("\n");
            sb.Append("  AgentName: ").Append(AgentName).Append("\n");
            sb.Append("  CronExpression: ").Append(CronExpression).Append("\n");
            sb.Append("  LastExecution: ").Append(LastExecution).Append("\n");
            sb.Append("  NextExecution: ").Append(NextExecution).Append("\n");
            sb.Append("  IsDisabled: ").Append(IsDisabled).Append("\n");
            sb.Append("  ProjectId: ").Append(ProjectId).Append("\n");
            sb.Append("  ProcessId: ").Append(ProcessId).Append("\n");
            sb.Append("  ProcessName: ").Append(ProcessName).Append("\n");
            sb.Append("  TriggerName: ").Append(TriggerName).Append("\n");
            sb.Append("  StartingType: ").Append(StartingType).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
            sb.Append("  ExpiryDate: ").Append(ExpiryDate).Append("\n");
            sb.Append("  StartDate: ").Append(StartDate).Append("\n");
            sb.Append("  CreatedBy: ").Append(CreatedBy).Append("\n");
            sb.Append("  ScheduleNow: ").Append(ScheduleNow).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as ScheduleViewModel);
        }

        /// <summary>
        /// Returns true if ScheduleViewModel instances are equal
        /// </summary>
        /// <param name="input">Instance of ScheduleViewModel to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ScheduleViewModel input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.CreatedOn == input.CreatedOn ||
                    (this.CreatedOn != null &&
                    this.CreatedOn.Equals(input.CreatedOn))
                ) && 
                (
                    this.Id == input.Id ||
                    (this.Id != null &&
                    this.Id.Equals(input.Id))
                ) && 
                (
                    this.Name == input.Name ||
                    (this.Name != null &&
                    this.Name.Equals(input.Name))
                ) && 
                (
                    this.AgentId == input.AgentId ||
                    (this.AgentId != null &&
                    this.AgentId.Equals(input.AgentId))
                ) && 
                (
                    this.AgentName == input.AgentName ||
                    (this.AgentName != null &&
                    this.AgentName.Equals(input.AgentName))
                ) && 
                (
                    this.CronExpression == input.CronExpression ||
                    (this.CronExpression != null &&
                    this.CronExpression.Equals(input.CronExpression))
                ) && 
                (
                    this.LastExecution == input.LastExecution ||
                    (this.LastExecution != null &&
                    this.LastExecution.Equals(input.LastExecution))
                ) && 
                (
                    this.NextExecution == input.NextExecution ||
                    (this.NextExecution != null &&
                    this.NextExecution.Equals(input.NextExecution))
                ) && 
                (
                    this.IsDisabled == input.IsDisabled ||
                    (this.IsDisabled != null &&
                    this.IsDisabled.Equals(input.IsDisabled))
                ) && 
                (
                    this.ProjectId == input.ProjectId ||
                    (this.ProjectId != null &&
                    this.ProjectId.Equals(input.ProjectId))
                ) && 
                (
                    this.ProcessId == input.ProcessId ||
                    (this.ProcessId != null &&
                    this.ProcessId.Equals(input.ProcessId))
                ) && 
                (
                    this.ProcessName == input.ProcessName ||
                    (this.ProcessName != null &&
                    this.ProcessName.Equals(input.ProcessName))
                ) && 
                (
                    this.TriggerName == input.TriggerName ||
                    (this.TriggerName != null &&
                    this.TriggerName.Equals(input.TriggerName))
                ) && 
                (
                    this.StartingType == input.StartingType ||
                    (this.StartingType != null &&
                    this.StartingType.Equals(input.StartingType))
                ) && 
                (
                    this.Status == input.Status ||
                    (this.Status != null &&
                    this.Status.Equals(input.Status))
                ) && 
                (
                    this.ExpiryDate == input.ExpiryDate ||
                    (this.ExpiryDate != null &&
                    this.ExpiryDate.Equals(input.ExpiryDate))
                ) && 
                (
                    this.StartDate == input.StartDate ||
                    (this.StartDate != null &&
                    this.StartDate.Equals(input.StartDate))
                ) && 
                (
                    this.CreatedBy == input.CreatedBy ||
                    (this.CreatedBy != null &&
                    this.CreatedBy.Equals(input.CreatedBy))
                ) && 
                (
                    this.ScheduleNow == input.ScheduleNow ||
                    (this.ScheduleNow != null &&
                    this.ScheduleNow.Equals(input.ScheduleNow))
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
                int hashCode = 41;
                if (this.CreatedOn != null)
                    hashCode = hashCode * 59 + this.CreatedOn.GetHashCode();
                if (this.Id != null)
                    hashCode = hashCode * 59 + this.Id.GetHashCode();
                if (this.Name != null)
                    hashCode = hashCode * 59 + this.Name.GetHashCode();
                if (this.AgentId != null)
                    hashCode = hashCode * 59 + this.AgentId.GetHashCode();
                if (this.AgentName != null)
                    hashCode = hashCode * 59 + this.AgentName.GetHashCode();
                if (this.CronExpression != null)
                    hashCode = hashCode * 59 + this.CronExpression.GetHashCode();
                if (this.LastExecution != null)
                    hashCode = hashCode * 59 + this.LastExecution.GetHashCode();
                if (this.NextExecution != null)
                    hashCode = hashCode * 59 + this.NextExecution.GetHashCode();
                if (this.IsDisabled != null)
                    hashCode = hashCode * 59 + this.IsDisabled.GetHashCode();
                if (this.ProjectId != null)
                    hashCode = hashCode * 59 + this.ProjectId.GetHashCode();
                if (this.ProcessId != null)
                    hashCode = hashCode * 59 + this.ProcessId.GetHashCode();
                if (this.ProcessName != null)
                    hashCode = hashCode * 59 + this.ProcessName.GetHashCode();
                if (this.TriggerName != null)
                    hashCode = hashCode * 59 + this.TriggerName.GetHashCode();
                if (this.StartingType != null)
                    hashCode = hashCode * 59 + this.StartingType.GetHashCode();
                if (this.Status != null)
                    hashCode = hashCode * 59 + this.Status.GetHashCode();
                if (this.ExpiryDate != null)
                    hashCode = hashCode * 59 + this.ExpiryDate.GetHashCode();
                if (this.StartDate != null)
                    hashCode = hashCode * 59 + this.StartDate.GetHashCode();
                if (this.CreatedBy != null)
                    hashCode = hashCode * 59 + this.CreatedBy.GetHashCode();
                if (this.ScheduleNow != null)
                    hashCode = hashCode * 59 + this.ScheduleNow.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }
}
