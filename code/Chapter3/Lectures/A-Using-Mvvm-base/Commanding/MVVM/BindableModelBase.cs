﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO;
using System.Xml.Serialization;

namespace uoplib.mvvm
{
    public class BindableModelBase : MVVMBase, INotifyPropertyChanged
    {
        protected string filename;
        public string Filename { get => filename; set => filename = value; }

        //Serialise this instance to an XML file
        public void Save()
        {
            if (Filename != null)
            {
                Save(Filename);
            }
            else
            {
                throw new Exception("No filename set - either load or save with filename first");
            }
        }

        public void Save(string FileName)
        {
            this.Filename = Filename;
            using (var writer = new System.IO.StreamWriter(FileName))
            {
                var serializer = new XmlSerializer(this.GetType());
                serializer.Serialize(writer, this);
                writer.Flush();
            }
        }

        //Deserialise an XML file to a new instance of this type
        public static ModelType Load<ModelType>(string fn) where ModelType : BindableModelBase
        {
            try
            {
                using (FileStream stream = File.OpenRead(fn))
                {
                    var serializer = new XmlSerializer(typeof(ModelType));
                    ModelType m = serializer.Deserialize(stream) as ModelType;
                    m.Filename = fn;
                    return m;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
