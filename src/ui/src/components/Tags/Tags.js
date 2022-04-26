import { useState } from "react";
import style from "./Tags.module.css"

function Tags(props){
    const [tags, setTags] = useState(props.tags ? props.tags : ['tag']);

    const onKeyUpAddTag = (event) => {
        if (event.key === 'Enter' && event.target.value.length > 0) {
            const tagExists = tags.find(t => t === event.target.value);

            if (!tagExists) {
                setTags([
                    ...tags,
                    event.target.value
                ]);
                event.target.value = "";
            }
        }
    }

    const onClickAddTag = (event) => {
        if (event.detail === 1 && event.target.value.length > 0) {
            const tagExists = tags.find(t => t === event.target.value);

            if (!tagExists) {
                setTags([
                    ...tags,
                    event.target.value
                ]);
                event.target.value = "";
            }
        }
    }

    const deleteTag = (tagText) => {
        const tagsFiltered = tags.filter(t => t !== tagText);
        setTags(tagsFiltered);
    }

    return (
        <div className={style.container}>
            <label>Tagi</label>
            <div className={style.tagContainer}>
                {tags.map(t => (
                    <div className={style.tag}>
                        <span>{t}</span>
                        <span className={style.xButton}
                              onClick={() => deleteTag(t)}>x</span>
                    </div>
                ))}
                <input onKeyUp={onKeyUpAddTag} 
                       onClick={onClickAddTag}/>
            </div>
        </div>
    )
}

export default Tags;