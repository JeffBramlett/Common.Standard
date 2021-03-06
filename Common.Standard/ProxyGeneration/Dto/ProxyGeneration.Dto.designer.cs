// ------------------------------------------------------------------------------
//  <auto-generated>
//    Generated by Xsd2Code. Version 3.4.0.15718 Microsoft Reciprocal License (Ms-RL) 
//    <NameSpace>ProxyGeneration.Dto</NameSpace><Collection>List</Collection><codeType>CSharp</codeType><EnableDataBinding>True</EnableDataBinding><EnableLazyLoading>False</EnableLazyLoading><TrackingChangesEnable>False</TrackingChangesEnable><GenTrackingClasses>False</GenTrackingClasses><HidePrivateFieldInIDE>False</HidePrivateFieldInIDE><EnableSummaryComment>True</EnableSummaryComment><VirtualProp>False</VirtualProp><IncludeSerializeMethod>False</IncludeSerializeMethod><UseBaseClass>False</UseBaseClass><GenBaseClass>False</GenBaseClass><GenerateCloneMethod>False</GenerateCloneMethod><GenerateDataContracts>False</GenerateDataContracts><CodeBaseTag>Net20</CodeBaseTag><SerializeMethodName>Serialize</SerializeMethodName><DeserializeMethodName>Deserialize</DeserializeMethodName><SaveToFileMethodName>SaveToFile</SaveToFileMethodName><LoadFromFileMethodName>LoadFromFile</LoadFromFileMethodName><GenerateXMLAttributes>False</GenerateXMLAttributes><OrderXMLAttrib>False</OrderXMLAttrib><EnableEncoding>False</EnableEncoding><AutomaticProperties>False</AutomaticProperties><GenerateShouldSerialize>False</GenerateShouldSerialize><DisableDebug>False</DisableDebug><PropNameSpecified>Default</PropNameSpecified><Encoder>UTF8</Encoder><CustomUsings></CustomUsings><ExcludeIncludedTypes>False</ExcludeIncludedTypes><EnableInitializeFields>True</EnableInitializeFields>
//  </auto-generated>
// ------------------------------------------------------------------------------
namespace ProxyGeneration.Dto {
    using System;
    using System.Diagnostics;
    using System.Xml.Serialization;
    using System.Collections;
    using System.Xml.Schema;
    using System.ComponentModel;
    using System.Collections.Generic;
    
    
    public partial class ProxyExtraction : System.ComponentModel.INotifyPropertyChanged {
        
        private System.DateTime timestampField;
        
        private string sourceAssemblyField;
        
        private List<TypeExtraction> typeExtractionsField;
        
        /// <summary>
        /// ProxyExtraction class constructor
        /// </summary>
        public ProxyExtraction() {
            this.typeExtractionsField = new List<TypeExtraction>();
        }
        
        public System.DateTime Timestamp {
            get {
                return this.timestampField;
            }
            set {
                if ((timestampField.Equals(value) != true)) {
                    this.timestampField = value;
                    this.OnPropertyChanged("Timestamp");
                }
            }
        }
        
        public string SourceAssembly {
            get {
                return this.sourceAssemblyField;
            }
            set {
                if ((this.sourceAssemblyField != null)) {
                    if ((sourceAssemblyField.Equals(value) != true)) {
                        this.sourceAssemblyField = value;
                        this.OnPropertyChanged("SourceAssembly");
                    }
                }
                else {
                    this.sourceAssemblyField = value;
                    this.OnPropertyChanged("SourceAssembly");
                }
            }
        }
        
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public List<TypeExtraction> TypeExtractions {
            get {
                return this.typeExtractionsField;
            }
            set {
                if ((this.typeExtractionsField != null)) {
                    if ((typeExtractionsField.Equals(value) != true)) {
                        this.typeExtractionsField = value;
                        this.OnPropertyChanged("TypeExtractions");
                    }
                }
                else {
                    this.typeExtractionsField = value;
                    this.OnPropertyChanged("TypeExtractions");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        public virtual void OnPropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler handler = this.PropertyChanged;
            if ((handler != null)) {
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    public partial class TypeExtraction : System.ComponentModel.INotifyPropertyChanged {
        
        private string namespaceField;
        
        private string nameField;
        
        private List<FieldExtraction> fieldsField;
        
        private List<PropertyExtraction> propertiesField;
        
        private List<MethodExtraction> methodsField;
        
        /// <summary>
        /// TypeExtraction class constructor
        /// </summary>
        public TypeExtraction() {
            this.methodsField = new List<MethodExtraction>();
            this.propertiesField = new List<PropertyExtraction>();
            this.fieldsField = new List<FieldExtraction>();
        }
        
        public string Namespace {
            get {
                return this.namespaceField;
            }
            set {
                if ((this.namespaceField != null)) {
                    if ((namespaceField.Equals(value) != true)) {
                        this.namespaceField = value;
                        this.OnPropertyChanged("Namespace");
                    }
                }
                else {
                    this.namespaceField = value;
                    this.OnPropertyChanged("Namespace");
                }
            }
        }
        
        public string Name {
            get {
                return this.nameField;
            }
            set {
                if ((this.nameField != null)) {
                    if ((nameField.Equals(value) != true)) {
                        this.nameField = value;
                        this.OnPropertyChanged("Name");
                    }
                }
                else {
                    this.nameField = value;
                    this.OnPropertyChanged("Name");
                }
            }
        }
        
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public List<FieldExtraction> Fields {
            get {
                return this.fieldsField;
            }
            set {
                if ((this.fieldsField != null)) {
                    if ((fieldsField.Equals(value) != true)) {
                        this.fieldsField = value;
                        this.OnPropertyChanged("Fields");
                    }
                }
                else {
                    this.fieldsField = value;
                    this.OnPropertyChanged("Fields");
                }
            }
        }
        
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public List<PropertyExtraction> Properties {
            get {
                return this.propertiesField;
            }
            set {
                if ((this.propertiesField != null)) {
                    if ((propertiesField.Equals(value) != true)) {
                        this.propertiesField = value;
                        this.OnPropertyChanged("Properties");
                    }
                }
                else {
                    this.propertiesField = value;
                    this.OnPropertyChanged("Properties");
                }
            }
        }
        
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public List<MethodExtraction> Methods {
            get {
                return this.methodsField;
            }
            set {
                if ((this.methodsField != null)) {
                    if ((methodsField.Equals(value) != true)) {
                        this.methodsField = value;
                        this.OnPropertyChanged("Methods");
                    }
                }
                else {
                    this.methodsField = value;
                    this.OnPropertyChanged("Methods");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        public virtual void OnPropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler handler = this.PropertyChanged;
            if ((handler != null)) {
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    public partial class FieldExtraction : System.ComponentModel.INotifyPropertyChanged {
        
        private string nameField;
        
        private string typeNameField;
        
        private string currentValueField;
        
        public string Name {
            get {
                return this.nameField;
            }
            set {
                if ((this.nameField != null)) {
                    if ((nameField.Equals(value) != true)) {
                        this.nameField = value;
                        this.OnPropertyChanged("Name");
                    }
                }
                else {
                    this.nameField = value;
                    this.OnPropertyChanged("Name");
                }
            }
        }
        
        public string TypeName {
            get {
                return this.typeNameField;
            }
            set {
                if ((this.typeNameField != null)) {
                    if ((typeNameField.Equals(value) != true)) {
                        this.typeNameField = value;
                        this.OnPropertyChanged("TypeName");
                    }
                }
                else {
                    this.typeNameField = value;
                    this.OnPropertyChanged("TypeName");
                }
            }
        }
        
        public string CurrentValue {
            get {
                return this.currentValueField;
            }
            set {
                if ((this.currentValueField != null)) {
                    if ((currentValueField.Equals(value) != true)) {
                        this.currentValueField = value;
                        this.OnPropertyChanged("CurrentValue");
                    }
                }
                else {
                    this.currentValueField = value;
                    this.OnPropertyChanged("CurrentValue");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        public virtual void OnPropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler handler = this.PropertyChanged;
            if ((handler != null)) {
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    public partial class MethodArgument : System.ComponentModel.INotifyPropertyChanged {
        
        private string nameField;
        
        private string typeField;
        
        private bool isParamsField;
        
        public string Name {
            get {
                return this.nameField;
            }
            set {
                if ((this.nameField != null)) {
                    if ((nameField.Equals(value) != true)) {
                        this.nameField = value;
                        this.OnPropertyChanged("Name");
                    }
                }
                else {
                    this.nameField = value;
                    this.OnPropertyChanged("Name");
                }
            }
        }
        
        public string Type {
            get {
                return this.typeField;
            }
            set {
                if ((this.typeField != null)) {
                    if ((typeField.Equals(value) != true)) {
                        this.typeField = value;
                        this.OnPropertyChanged("Type");
                    }
                }
                else {
                    this.typeField = value;
                    this.OnPropertyChanged("Type");
                }
            }
        }
        
        public bool IsParams {
            get {
                return this.isParamsField;
            }
            set {
                if ((isParamsField.Equals(value) != true)) {
                    this.isParamsField = value;
                    this.OnPropertyChanged("IsParams");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        public virtual void OnPropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler handler = this.PropertyChanged;
            if ((handler != null)) {
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    public partial class MethodExtraction : System.ComponentModel.INotifyPropertyChanged {
        
        private string nameField;
        
        private string returnValueTypeField;
        
        private string signatureField;
        
        private List<MethodArgument> argumentsField;
        
        /// <summary>
        /// MethodExtraction class constructor
        /// </summary>
        public MethodExtraction() {
            this.argumentsField = new List<MethodArgument>();
        }
        
        public string Name {
            get {
                return this.nameField;
            }
            set {
                if ((this.nameField != null)) {
                    if ((nameField.Equals(value) != true)) {
                        this.nameField = value;
                        this.OnPropertyChanged("Name");
                    }
                }
                else {
                    this.nameField = value;
                    this.OnPropertyChanged("Name");
                }
            }
        }
        
        public string ReturnValueType {
            get {
                return this.returnValueTypeField;
            }
            set {
                if ((this.returnValueTypeField != null)) {
                    if ((returnValueTypeField.Equals(value) != true)) {
                        this.returnValueTypeField = value;
                        this.OnPropertyChanged("ReturnValueType");
                    }
                }
                else {
                    this.returnValueTypeField = value;
                    this.OnPropertyChanged("ReturnValueType");
                }
            }
        }
        
        public string Signature {
            get {
                return this.signatureField;
            }
            set {
                if ((this.signatureField != null)) {
                    if ((signatureField.Equals(value) != true)) {
                        this.signatureField = value;
                        this.OnPropertyChanged("Signature");
                    }
                }
                else {
                    this.signatureField = value;
                    this.OnPropertyChanged("Signature");
                }
            }
        }
        
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public List<MethodArgument> Arguments {
            get {
                return this.argumentsField;
            }
            set {
                if ((this.argumentsField != null)) {
                    if ((argumentsField.Equals(value) != true)) {
                        this.argumentsField = value;
                        this.OnPropertyChanged("Arguments");
                    }
                }
                else {
                    this.argumentsField = value;
                    this.OnPropertyChanged("Arguments");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        public virtual void OnPropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler handler = this.PropertyChanged;
            if ((handler != null)) {
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    public partial class PropertyExtraction : System.ComponentModel.INotifyPropertyChanged {
        
        private string nameField;
        
        private string typeNameField;
        
        private bool hasGetField;
        
        private bool hasSetField;
        
        public string Name {
            get {
                return this.nameField;
            }
            set {
                if ((this.nameField != null)) {
                    if ((nameField.Equals(value) != true)) {
                        this.nameField = value;
                        this.OnPropertyChanged("Name");
                    }
                }
                else {
                    this.nameField = value;
                    this.OnPropertyChanged("Name");
                }
            }
        }
        
        public string TypeName {
            get {
                return this.typeNameField;
            }
            set {
                if ((this.typeNameField != null)) {
                    if ((typeNameField.Equals(value) != true)) {
                        this.typeNameField = value;
                        this.OnPropertyChanged("TypeName");
                    }
                }
                else {
                    this.typeNameField = value;
                    this.OnPropertyChanged("TypeName");
                }
            }
        }
        
        public bool HasGet {
            get {
                return this.hasGetField;
            }
            set {
                if ((hasGetField.Equals(value) != true)) {
                    this.hasGetField = value;
                    this.OnPropertyChanged("HasGet");
                }
            }
        }
        
        public bool HasSet {
            get {
                return this.hasSetField;
            }
            set {
                if ((hasSetField.Equals(value) != true)) {
                    this.hasSetField = value;
                    this.OnPropertyChanged("HasSet");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        public virtual void OnPropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler handler = this.PropertyChanged;
            if ((handler != null)) {
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
